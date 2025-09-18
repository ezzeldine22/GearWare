using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.AccountDTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "The Email Or Phone field is required.")]
        public string emailOrPhone { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        public string password { get; set; }

        [Required(ErrorMessage = "The Model field is required.")]
        public bool remember { get; set; }
    }
}
