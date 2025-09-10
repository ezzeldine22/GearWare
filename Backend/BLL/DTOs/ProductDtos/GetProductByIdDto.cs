using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.ProductDtos
{
    public class GetProductByIdDto
    {
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductCategory { get; set; }

        public decimal ProductPrice { get; set; }

        public int ProductStockQuantity { get; set; }

        public string CategoryName { get; set; }
    }
}
