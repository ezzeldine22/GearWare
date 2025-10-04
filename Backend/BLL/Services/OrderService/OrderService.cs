using BLL.DTOs.CartDtos;
using BLL.DTOs.OrderDtos;
using BLL.Exceptions;
using CleanArchitecture.Core.Interfaces;
using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<Order> _orderRepo;

        public OrderService(IRepository<OrderItem> OrderItemRepo,IRepository<Order> orderRepo)
        {
            _orderItemRepo = OrderItemRepo;
            _orderRepo = orderRepo;
        }

        public IEnumerable<GetAllOrdersDto> GetAllOrders(string ClientId)
        {
          var AllOrders = _orderRepo.ReadAll().Where(o=> o.UserId == ClientId);

            if (AllOrders == null)
            {
                throw new CustomException(new List<string> { "No Order To Show !!!" });
            }
            
            var AllOrderDto = AllOrders.Select(o => new GetAllOrdersDto
            {
              Status = o.Status,
              Date = o.OrderDateUtc,
              OrderId = o.OrderId,
              Total = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
            }).ToList();

            return AllOrderDto;
        }

        public IEnumerable<GetAllOrderItemsDto> GetAllOrderItems(int orderId)
        {
            var AllOrderItems = _orderItemRepo.ReadAll().Where(oi => oi.OrderId == orderId);

            if (AllOrderItems == null)
            {
                throw new CustomException(new List<string> { "No OrderItems To Show !!!" });
            }
          
            var AllOrderItemsDto = AllOrderItems.Select(c => new GetAllOrderItemsDto
            {
                OrderItemDescription = c.Product.Description,
                OrderItemPrice = c.UnitPrice,
                OrderItemQuantity = c.Quantity,

            }).ToList();

            return AllOrderItemsDto;
        }
    
    }
}
