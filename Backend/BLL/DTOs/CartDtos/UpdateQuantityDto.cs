using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CartDtos
{
    public class UpdateQuantityDto
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
