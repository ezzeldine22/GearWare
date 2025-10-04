using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CategoryDtos
{
    public class AddCategoryDto
    {
        [Required(ErrorMessage = "This is A Required Field")]
        public string categoryName { get; set; }

        [Required(ErrorMessage = "This is A Required Field")]
        public string categoryDescription { get; set; }

        [Required(ErrorMessage = "This is A Required Field")]
        public string imgUrl { get; set; }
    }
}
