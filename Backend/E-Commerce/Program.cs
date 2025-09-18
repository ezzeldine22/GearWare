using BLL.DTOs.ProductDtos;
using BLL.Managers.AccountManager;
using BLL.Services.CartServices;
using BLL.Services.CategoryService;
using BLL.Services.CategoryService.CategoryService;
using BLL.Services.OrderService;
using BLL.Services.ProductServices;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using DAL.Data;
using DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();



            builder.Services.AddDbContext<EcommerceDbContext>(Option =>
            Option.UseSqlServer(builder.Configuration.GetConnectionString("EDB"))
            );

            //------------------------------------------------------//


            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<ICartService ,CartService>();
            builder.Services.AddScoped<IOrderService ,OrderService>();
            builder.Services.AddScoped<IAccountManager, AccountManager>();

            //------------------------------------------------------//

            builder.Services.AddIdentity<User, IdentityRole>(options => { })
            .AddEntityFrameworkStores<EcommerceDbContext>();
            

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

            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
