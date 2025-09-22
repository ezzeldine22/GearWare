using BLL.DTOs.ProductDtos;
using BLL.Exceptions;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Infrastructure.Persistence;
using DAL.Data;
using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BLL.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _ProductRepo;
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<ProductImage> _poductImageRepo;

        public ProductService(IRepository<Product> ProductRepo, IRepository<Category> CategoryRepo , IRepository<ProductImage> poductImageRepo)
        {
            _ProductRepo = ProductRepo;
            _categoryRepo = CategoryRepo;
            _poductImageRepo = poductImageRepo;
        }

        public async Task AddProductAsync(AddProductDto _addProductDto)
        {
            var ProductCheck = await _ProductRepo.FirstOrDefaultAsync(e => e.Name == _addProductDto.ProductName);


            if (ProductCheck != null)
            {
                throw new CustomException(new List<string> { "The Product Already Exists !!!" });
            }

            var AddedProductCategory = (await _categoryRepo.FirstOrDefaultAsync(c => c.Name == _addProductDto.ProductCategoryName));

            var AddedProduct = new Product
            {
                Name = _addProductDto.ProductName,
                Price = _addProductDto.ProductPrice,
                Description = _addProductDto.ProductDescription,
                StockQuantity = _addProductDto.ProductStockQuantity,
                CategoryId = AddedProductCategory.CategoryId,
            
            };

            await _ProductRepo.AddAsync(AddedProduct);

            var ProductImage = new ProductImage
            {
                ImageUrl = _addProductDto.Imageurl,
                Product = AddedProduct,
            };

            await _poductImageRepo.AddAsync(ProductImage);
            _ProductRepo.SaveChanges();

        }

        public async Task DeleteProductAsync(int Id)
        {

            await _ProductRepo.DeleteAsync(Id);
            _ProductRepo.SaveChanges();

        }

        public async Task EditProductAsync(EditProductDto editProductDto)
        {
            var EditedProduct = await _ProductRepo.ReadById(editProductDto.ProductId);
            var Category = await _categoryRepo.FirstOrDefaultAsync(c => c.Name == editProductDto.ProductCategoryName);
            var editedImage = await _poductImageRepo.FirstOrDefaultAsync(pi=>pi.ProductId == editProductDto.ProductId);

            EditedProduct.Name = editProductDto.ProductName;
            EditedProduct.Price = editProductDto.ProductPrice;
            EditedProduct.Description = editProductDto.ProductDescription;
            EditedProduct.CategoryId = Category.CategoryId;
            EditedProduct.StockQuantity = editProductDto.ProductStockQuantity;
            editedImage.ImageUrl = editProductDto.imageUrl;


            _poductImageRepo.UpdateAsync(editedImage);
            _ProductRepo.UpdateAsync(EditedProduct);
            _ProductRepo.SaveChanges();
        }

        public IEnumerable<GetAllProductsDto> GetAllProducts()
        {
            var AllProducts = _ProductRepo.ReadAll()
                .Include(p => p.Images).Include(p => p.Category)
                .ToList();

            return GetAll(AllProducts);

        }

        public static IEnumerable<GetAllProductsDto> GetAll(IEnumerable<Product> entity)
        {
            if (!entity.Any())
            {
                throw new CustomException(new List<string> { "No Products To Show !!!" });
            }

            var AllProductsDto = entity.Select(p => new GetAllProductsDto
            {
                ProductName = p.Name,
                Id = p.ProductId,
                ProductPrice = p.Price,
                ProductDescription = p.Description,
                ProductStockQuantity = p.StockQuantity,
                ProductCategory = p.Category.Name,
                Images = p.Images.Select(p => p.ImageUrl)

            }).ToList();

            return AllProductsDto;
        }

        public async Task<GetProductByIdDto> GetProductByIdAsync(int Id)
        {
            var ProductById = await _ProductRepo.ReadAll().Include(p => p.Category).Include(p => p.Images).FirstOrDefaultAsync(p => p.ProductId == Id);

            if (ProductById == null)
            {
                throw new CustomException(new List<string> { "No Products To Show !!!" });
            }


            var ProductByIdDto = new GetProductByIdDto
            {
                ProductName = ProductById.Name,
                ProductPrice = ProductById.Price,
                ProductDescription = ProductById.Description,
                ProductStockQuantity = ProductById.StockQuantity,
                ProductCategory = ProductById.Category.Name,
                ProductImages = ProductById.Images.Select(i => i.ImageUrl)
            };

            return ProductByIdDto;
        }

        public async Task<IEnumerable<GetAllProductsDto>> SearchProductsPagedAsync(string query, int pageNumber, ProductSortBy sortBy = ProductSortBy.Latest)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new CustomException(new List<string> { "Please enter something !!!" });
            }

            var EnhancedQuery = query.Trim();

           
            var ProductQuery = _ProductRepo.ReadAll()
                .Include(p => p.Category)
                .Include(p => p.Reviews) 
                .Where(p =>
                    EF.Functions.Like(p.Name, $"{EnhancedQuery}%") ||
                    EF.Functions.Like(p.Description, $"%{EnhancedQuery}%") ||
                    EF.Functions.Like(p.Category.Name, $"{EnhancedQuery}%")
                );

            ProductQuery = sortBy switch
            {
                ProductSortBy.PriceLowToHigh => ProductQuery.OrderBy(p => p.Price),
                ProductSortBy.PriceHighToLow => ProductQuery.OrderByDescending(p => p.Price),
                ProductSortBy.Rating => ProductQuery
                .OrderByDescending(p => p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0),

                _ => ProductQuery.OrderByDescending(p => p.CreatedAtUtc),
            };

            var Matched = await ProductQuery
               .Skip((pageNumber - 1) * 16)
               .Take(16)
               .ToListAsync();

            if (!Matched.Any())
            {
                throw new CustomException(new List<string> { "No Products Found !!!" });
            }

            return GetAll(Matched);

        }
    }
}
