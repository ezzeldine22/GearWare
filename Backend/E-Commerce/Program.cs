using BLL.DTOs.ProductDtos;
using BLL.Services.CartServices;
using BLL.Services.CategoryService;
using BLL.Services.CategoryService.CategoryService;
using BLL.Services.OrderService;
using BLL.Services.ProductServices;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using DAL.Data;
using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

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

            //builder.Services.AddDbContext<EcommerceDbContext>(Option =>
            //Option.UseSqlServer("Server=.;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True;"));



            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryServices, CategoryServices>();
            builder.Services.AddScoped<ICartService ,CartService>();
            builder.Services.AddScoped<IOrderService ,OrderService>();
            //builder.Services.AddScoped<GetAllProductsDto>();
            //builder.Services.AddScoped<AddProductDto>();
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
