using BLL.DTOs.ProductDtos;
using BLL.Exceptions;
using BLL.Services.ProductService;
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
       
        //public async Task<IEnumerable<Product>> GetAllProducts(GetAllProductsDto productDto)
        //{
        //    try
        //    {
        //        await productService.GetAllProductsAsync(productDto);
        //        return Ok(Result);

        //    }
        //    catch (CustomException ex)
        //    {
        //        return BadRequest(new
        //        {
        //            ex.Errors
        //        });
        //    }

        //}
        
        [HttpPost("")]
        
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
    }
}
