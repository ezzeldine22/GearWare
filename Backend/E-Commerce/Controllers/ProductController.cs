using BLL.DTOs.ProductDtos;
using BLL.Exceptions;
using BLL.Services.ProductServices;
using DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
  
    public class ProductController : ControllerBase
    {

        private readonly IProductService productService;
   

        public ProductController(IProductService ProductService)
        {
            productService = ProductService;
       
        }

        [HttpGet("")]
       

        public ActionResult<IEnumerable<GetAllProductsDto>> GetAllProducts()
        {
            try
            {
                return Ok(productService.GetAllProducts());
            }
            catch (CustomException ex)
            {
                return BadRequest(ex.Errors);
            }

        }
        
        [HttpPost("")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddProduct(AddProductDto _addProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await productService.AddProductAsync(_addProductDto);
                return Ok();
            }
            catch (CustomException ex)
            {
                return BadRequest(new {
                    ex.Errors
                });
            }
        }


        [HttpDelete("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduct(int Id)
        {
            await productService.DeleteProductAsync(Id);
            return Ok();
        }

        [HttpGet("{Id}")]
    
        public async Task<ActionResult<GetProductByIdDto>> GetById(int Id)
        {
            var product = await productService.GetProductByIdAsync(Id);
            return Ok(product);
        }

        [HttpPut("")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditProduct(EditProductDto editProductDto)
        {
            await productService.EditProductAsync(editProductDto);
            return Ok();
        }

        [HttpGet("search")]
       
        public async Task<ActionResult<IEnumerable<GetAllProductsDto>>> Search(string query, int pageNumber = 1 , ProductSortBy sortBy = ProductSortBy.Latest)
        {
            try
            {
                var result = await productService.SearchProductsPagedAsync(query, pageNumber,sortBy);
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return BadRequest(new
                {
                    ex.Errors
                });
            }

        }
    }
}
