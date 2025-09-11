using DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.ProductDtos
{
    public class AddProductDto
    {
        [Required(ErrorMessage = "This is A Required Field")]
        
        public string ProductName { get; set; }
        [Required(ErrorMessage = "This is A Required Field")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "This is A Required Field")]
        public decimal ProductPrice { get; set; }
        [Required(ErrorMessage = "This is A Required Field")]
        public int ProductStockQuantity { get; set; }
        [Required(ErrorMessage = "This is A Required Field")]
        public string  ProductCategoryName { get; set; }



        

    }
}
