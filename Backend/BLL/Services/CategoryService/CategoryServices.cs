using BLL.DTOs.CategoryDtos;
using BLL.DTOs.ProductDtos;
using BLL.Exceptions;
using BLL.Services.CategoryService.CategoryService;
using CleanArchitecture.Core.Interfaces;
using DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.ProductServices;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.CategoryService
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IRepository<Category> _categoryRepo;
        private readonly IRepository<Product> _productRepo;

        public CategoryServices(IRepository<Category> categoryRepo, IRepository<Product> productRepo)
        {
            _categoryRepo = categoryRepo;
            _productRepo = productRepo;
        }

        public async Task AddCategoryAsync(AddCategoryDto _addCategoryDto)
        {
            var categoryCheck = await _categoryRepo.FirstOrDefaultAsync(e => e.Name == _addCategoryDto.categoryName);


            if (categoryCheck != null)
            {
                throw new CustomException(new List<string> { "The Category Already Exists !!!" });
            }

            var AddCategory = new Category
            {
                Name = _addCategoryDto.categoryName,
                Description = _addCategoryDto.categoryDescription
            };

            await _categoryRepo.AddAsync(AddCategory);
            _categoryRepo.SaveChanges();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepo.DeleteAsync(id);
            _categoryRepo.SaveChanges();
        }

        public async Task EditCategoryAsync(EditCategoryDto editCategoryDto)
        {
            var EditedCategory = await _categoryRepo.ReadById(editCategoryDto.categoryId);

            EditedCategory.Name = editCategoryDto.categoryName;
            EditedCategory.Description = editCategoryDto.categoryDescription;

            _categoryRepo.UpdateAsync(EditedCategory);
            _categoryRepo.SaveChanges();
        }

        public IEnumerable<GetAllCategoriesDto> GetAllCategories()
        {
            var AllCategory = _categoryRepo.ReadAll();

            if (AllCategory == null)
            {
                throw new CustomException(new List<string> { "No Categories To Show !!!" });
            }

            var AllCategoriesDto = AllCategory.Select(c => new GetAllCategoriesDto
            {
                categoryId = c.CategoryId,
                categoryName = c.Name,
                categoryDescription = c.Description,
            }).ToList();

            return AllCategoriesDto;
        }

        public IEnumerable<GetAllProductsDto> GetCategoryByIdAsync(int Id)
        {
            var allProduct = _productRepo.ReadAll()
                .Include(p => p.Category)
                .ToList();

            var categoryProduct = allProduct.Where(p => p.CategoryId == Id);

            return ProductService.GetAll(categoryProduct);
        }
    }
}
