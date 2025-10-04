using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.ReviewDTOs
{
    public class GetAllReviewsDto
    {
        public float avrRate { get; set; }
        public IEnumerable<GetReviewDto>? comments { get; set; }
    }
}
