using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.ReviewDTOs
{
    public class AddReviewDto
    {
        public float Rating { get; set; }

        public string? Comment { get; set; }
    }
}
