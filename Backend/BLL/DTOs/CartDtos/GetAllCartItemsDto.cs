using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CartDtos
{
    public class GetAllCartItemsDto
    {

        public string CartItemDescription { get; set; }

        public decimal CartItemPrice { get; set; }
    }
}
