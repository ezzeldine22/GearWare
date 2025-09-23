using BLL.DTOs.ProductDtos;
using BLL.Managers.AccountManager;
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
using System.Text;

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}