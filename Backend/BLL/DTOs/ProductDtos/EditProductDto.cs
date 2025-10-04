using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.ProductDtos
{
    public class EditProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductDescription { get; set; }

        public string imageUrl { get; set; }
        public int ProductStockQuantity { get; set; }

        public string ProductCategoryName { get; set; }


    }
}
