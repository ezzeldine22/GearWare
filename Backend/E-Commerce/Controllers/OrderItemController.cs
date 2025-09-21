using BLL.DTOs.OrderDtos;
using BLL.Services.OrderService;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize("")]
        public ActionResult<IEnumerable<GetAllOrderItemsDto>> GetAllOrderItems(int orderId)
        {
            var AllOrderItems = _orderService.GetAllOrderItems(orderId);
            return Ok(AllOrderItems);
        }

        [HttpGet("Get AllOrders")]
        [Authorize("")]
        public ActionResult<IEnumerable<GetAllOrdersDto>> GetAllOrders(string ClientId)
        {
            var AllOrders = _orderService.GetAllOrders(ClientId);
            return Ok(AllOrders);
        }
    }
}
