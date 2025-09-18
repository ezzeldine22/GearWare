using BLL.DTOs.OrderDtos;
using BLL.Services.OrderService;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderItemController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("Get Order Items")]
        public ActionResult<IEnumerable<GetAllOrderItemsDto>> GetAllOrderItems()
        {
            var AllOrderItems = _orderService.GetAllOrderItems();
            return Ok(AllOrderItems);
        }
    }
}
