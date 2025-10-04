﻿using BLL.DTOs.ProductDtos;
using BLL.Exceptions;
using BLL.Services.ProductServices;
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
        public async Task<ActionResult> DeleteProduct(int Id)
        {
            await productService.DeleteProductAsync(Id);
            return Ok();
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult> GetById(int Id)
        {
            var product = await productService.GetProductByIdAsync(Id);
            return Ok(product);
        }

        [HttpPut("")]
        public async Task<ActionResult> EditProduct(EditProductDto editProductDto)
        {
            await productService.EditProductAsync(editProductDto);
            return Ok();
        }
    }
}
