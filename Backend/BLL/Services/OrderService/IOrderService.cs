using BLL.DTOs.CartDtos;
using BLL.DTOs.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.OrderService
{
    public interface IOrderService
    {
        IEnumerable<GetAllOrderItemsDto> GetAllOrderItems(int orderId);
        IEnumerable<GetAllOrdersDto> GetAllOrders(string ClientId);
    }
}
