using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.AccountDTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "The Name field is required.")]
        public  string Name { get; set; }


        [Required(ErrorMessage = "The Phone field is required.")]
        public  string phone { get; set; }


        [Required(ErrorMessage = "The Email field is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The Email field is not a valid e-mail address.")]
        public string email { get; set; }

        [Required(ErrorMessage = "The Password field is required.")]
        [RegularExpression(@"^\d{6,}$", ErrorMessage = "Passwords must be at least 6 characters.")]
        public  string password { get; set; }


        [Required(ErrorMessage = "The gender field is required.")]
        public string gender { get; set; }

     
    }
}
