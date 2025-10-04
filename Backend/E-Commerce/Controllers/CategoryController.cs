using BLL.DTOs.CategoryDtos;
using BLL.DTOs.ProductDtos;
using BLL.Exceptions;
using BLL.Services.CategoryService.CategoryService;
using BLL.Services.ProductServices;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        [HttpGet("")]
        public ActionResult<GetAllCategoriesDto> GetAll()
        {
            try
            {
                return Ok(_categoryServices.GetAllCategories());
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<GetAllProductsDto> GetById(int id)
        {
            var category = _categoryServices.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        [HttpPost("")]
        public async Task<ActionResult> AddCategory(AddCategoryDto addCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _categoryServices.AddCategoryAsync(addCategoryDto);
                return Ok();
            }
            catch (CustomException ex)
            {
                return BadRequest(new
                {
                    ex.Errors
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _categoryServices.DeleteCategoryAsync(id);
            return Ok();
        }

        [HttpPut("")]
        public async Task<ActionResult> EditProduct(EditCategoryDto editCategoryDto)
        {
            await _categoryServices.EditCategoryAsync(editCategoryDto);
            return Ok();
        }
    }
}
