using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.WishListDtos
{
    public class AddItemToWishListDto
    {
        public string description { get; set; }
        public decimal price { get; set; }
    }
}
