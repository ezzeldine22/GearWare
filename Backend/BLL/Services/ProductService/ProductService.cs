using BLL.DTOs.ProductDtos;
using BLL.Exceptions;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _ProductRepo;
        private readonly IRepository<Category> _categoryRepo;

        public ProductService(IRepository<Product> ProductRepo, IRepository<Category> CategoryRepo)
        {
            _ProductRepo = ProductRepo;
            _categoryRepo = CategoryRepo;
        }

   
        public async Task AddProductAsync(AddProductDto _addProductDto)
        {
            var ProductCheck  = await _ProductRepo.FirstOrDefaultAsync(e => e.Name == _addProductDto.ProductName);


            if (ProductCheck != null)
            {
                throw new CustomException(new List<string> { "The Product Already Exists !!!"});
            }
            var AddedProductCategory = (await _categoryRepo.FirstOrDefaultAsync(c=>c.Name == _addProductDto.ProductCategoryName));

            var AddedProduct = new Product
            {
                Name = _addProductDto.ProductName,
                Price = _addProductDto.ProductPrice,
                Description = _addProductDto.ProductDescription,
                StockQuantity = _addProductDto.ProductStockQuantity,
                CategoryId = AddedProductCategory.CategoryId // combo box 

            };

             await _ProductRepo.AddAsync(AddedProduct);
            _ProductRepo.SaveChanges();

        }

        public Task DeleteProductAsync()
        {
            throw new NotImplementedException();
        }

        public Task EditProductAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GetAllProductsDto>> GetAllProductsAsync(GetAllProductsDto productDto)
        {
            var AllProducts = _ProductRepo.ReadAllAsync();
            var AllCategories = _categoryRepo.ReadAllAsync();
            
            if (AllProducts == null)
            {
                throw new CustomException(new List<string> { "No Products To Show !!!" });
            }
            
            var Filterd = AllCategories.Where(cat=>cat.CategoryId == )


            var AllProductsDto = AllProducts.Select(p => new GetAllProductsDto
            {
                ProductName = p.Name,
                ProductPrice = p.Price,
                ProductDescription = p.Description,
                ProductStockQuantity = p.StockQuantity,
                //ProductCategory = _categoryRepo.Select()
                
                

            }).ToList();

          
               

           return AllProductsDto;

        }

        public Task GetProductByIdAsync()
        {
            throw new NotImplementedException();
        }
    }
}
