using BLL.DTOs.ProductDtos;
using BLL.DTOs.WishListDtos;
using BLL.Exceptions;
using BLL.Services.WishListService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {
        private readonly IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        [HttpGet("{ClientId}")]

        public ActionResult<IEnumerable<GetAllWishListItemDtos>> GetAll(string ClientId)
        {
            try
            {
                var items = _wishListService.GetAllWishListItems(ClientId);
                return Ok(items);
            }
            catch(CustomException ex)
            {
                return BadRequest(ex.Errors);
            }
        }

        [HttpPost("")]
 
        public ActionResult AddItem(int productId, string ClientId)
        {
            _wishListService.AddItemToWishList(productId, ClientId);
            return Ok();
        }
    }
}
