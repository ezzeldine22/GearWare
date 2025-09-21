using BLL.DTOs.CartDtos;
using BLL.DTOs.ProductDtos;
using BLL.Services.CartServices;
using DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("")]
        [Authorize("")]
        public async Task<ActionResult> AddToCart(AddToCartDto addToCartDto)
        {
            await _cartService.AddToCart(addToCartDto);
            return Ok();
        }
        [HttpPut("")]
        [Authorize("")]
        public async Task<ActionResult> UpdateQunatity(UpdateQuantityDto updateQuantityDto)
        {
           await _cartService.UpdateQuantity(updateQuantityDto);
           return Ok();
        }
        [HttpGet("")]
        [Authorize("")]
        public ActionResult<IEnumerable<GetAllCartItemsDto>> GetAllCartItems(int cartId)
        {
           var AllCartItems = _cartService.GetAllCartItems(cartId);
            return Ok(AllCartItems);
        }
        [HttpDelete("{id}")]
        [Authorize("")]
        public async Task<ActionResult> DeleteCartItem(int id)
        {
            await _cartService.DeleteCartItem(id);
            return Ok();
        }
    }
}
