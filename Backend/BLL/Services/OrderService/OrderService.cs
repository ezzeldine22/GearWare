using BLL.DTOs.CartDtos;
using BLL.DTOs.OrderDtos;
using BLL.Exceptions;
using CleanArchitecture.Core.Interfaces;
using DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<OrderItem> _orderItemRepo;

        public OrderService(IRepository<OrderItem> OrderItemRepo)
        {
            _orderItemRepo = OrderItemRepo;
        }
        public IEnumerable<GetAllOrderItemsDto> GetAllOrderItems()
        {
            var AllOrderItems = _orderItemRepo.ReadAll();

            if (AllOrderItems == null)
            {
                throw new CustomException(new List<string> { "No CartItems To Show !!!" });
            }

            var AllOrderItemsDto = AllOrderItems.Select(c => new GetAllOrderItemsDto
            {
                OrderItemDescription = c.Product.Description,
                OrderItemPrice = c.Product.Price,

            }).ToList();

            return AllOrderItemsDto;
        }
    }
}
