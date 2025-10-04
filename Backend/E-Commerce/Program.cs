using BLL.DTOs.ProductDtos;
using BLL.Managers.AccountManager;
using BLL.Services.AccountManager;
using BLL.Services.CartServices;
using BLL.Services.CategoryService;
using BLL.Services.CategoryService.CategoryService;
using BLL.Services.OrderService;
using BLL.Services.ProductServices;
using BLL.Services.ReviewService;
using BLL.Services.WishListService;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using DAL.Data;
using DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace E_Commerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<EcommerceDbContext>(Option =>
                Option.UseSqlServer(builder.Configuration.GetConnectionString("EDB"))
            );

            //------------------------------------------------------//

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IAccountManager, AccountManager>();
            builder.Services.AddScoped<IWishListService, WishListService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<Logger<AccountController>>();

            //------------------------------------------------------//

            builder.Services.AddIdentity<User, IdentityRole>(options => { })
                .AddEntityFrameworkStores<EcommerceDbContext>();
            builder.Services.AddControllers(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .RequireRole("Admin", "Client")
                                .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "jwt";
                options.DefaultChallengeScheme = "jwt";
            }).AddJwtBearer("jwt", option =>
            {
                var SecretKey = builder.Configuration.GetSection("SecretKey").Value;
                var SecretKeyByte = Encoding.UTF8.GetBytes(SecretKey);
                SecurityKey securityKey = new SymmetricSecurityKey(SecretKeyByte);

                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            builder.Services.AddHttpClient("GoogleApi", client =>
            {
                client.BaseAddress = new Uri("https://people.googleapis.com/");
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            });
            //---------------------Google , Facebook ---------------//

            builder.Services.AddAuthentication().
                AddGoogle(options => // google options options --> oAuthoptions : used to build the oAuth flow
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                    options.Scope.Add("https://www.googleapis.com/auth/user.gender.read");


                    options.Events.OnCreatingTicket = async context =>
                    {
                        var logger = context.HttpContext.RequestServices
                        .GetRequiredService<ILogger<Program>>();
                        var accessToken = context.AccessToken;
                        if (string.IsNullOrEmpty(accessToken)) return;

                        try
                        {
                            var clientFactory = context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
                            var client = clientFactory.CreateClient("GoogleApi");
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                            using var response = await client.GetAsync("v1/people/me?personFields=genders");
                            if (!response.IsSuccessStatusCode)
                            {
                                logger.LogWarning(
                               $"Google API request failed. StatusCode: {response.StatusCode}, Reason: {response.ReasonPhrase}" 
                               );
                                return;
                            }

                            await using var stream = await response.Content.ReadAsStreamAsync();
                            using var doc = await JsonDocument.ParseAsync(stream);

                            if (doc.RootElement.TryGetProperty("genders", out var genders) &&
                                genders.ValueKind == JsonValueKind.Array &&
                                genders.GetArrayLength() > 0)
                            {
                                var first = genders[0];
                                if (first.TryGetProperty("value", out var val) && val.ValueKind == JsonValueKind.String)
                                {
                                    var gender = val.GetString();
                                    if (!string.IsNullOrEmpty(gender))
                                    {
                                        context.Identity.AddClaim(new Claim(ClaimTypes.Gender, gender));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred while fetching gender from Google People API");
                        }
                    };

                });

            builder.Services.AddAuthentication().AddFacebook(options =>
            {
                options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
                options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
                options.Scope.Add("email");
                options.Scope.Add("public_profile");
                options.Scope.Add("user_gender");
                options.Fields.Add("gender");
                
            });

            //-----------------------------------------------------//
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

               
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by a space and the JWT token."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

      
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

        
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

           
            app.UseCors("AllowAll");

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}