using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CartDtos
{
    public class AddToCartDto
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
