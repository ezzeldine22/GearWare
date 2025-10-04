using BLL.DTOs.CategoryDtos;
using BLL.DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.CategoryService.CategoryService
{
    public interface ICategoryServices
    {
        Task AddCategoryAsync(AddCategoryDto _addCategoryDto);
        Task DeleteCategoryAsync(int id);
        Task EditCategoryAsync(EditCategoryDto editCategoryDto);
        IEnumerable<GetAllCategoriesDto> GetAllCategories();
        IEnumerable<GetAllProductsDto> GetCategoryByIdAsync(int Id);
    }
}
