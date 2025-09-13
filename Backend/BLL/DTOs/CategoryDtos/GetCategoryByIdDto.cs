using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.CategoryDtos
{
    public class GetCategoryByIdDto
    {
        public string categoryName { get; set; }
        public string categoryDescription { get; set; }
        public DateTime categoryCreatedAtUtc { get; set; }
    }
}
