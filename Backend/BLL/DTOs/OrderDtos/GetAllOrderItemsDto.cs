using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.OrderDtos
{
    public class GetAllOrderItemsDto
    {
        public string OrderItemDescription { get; set; }

        public decimal OrderItemPrice { get; set; }

        public decimal OrderItemQuantity { get; set; }
    }
}
