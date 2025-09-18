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
        [Required(ErrorMessage = "The First Name field is required.")]
        public  string firstName { get; set; }


        [Required(ErrorMessage = "The Last Name field is required.")]
        public  string lastName { get; set; }


        [Required(ErrorMessage = "The Phone field is required.")]
        public  string phone { get; set; }


        [Required(ErrorMessage = "The Email field is required.")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "The Email field is not a valid e-mail address.")]
        //[RegularExpression(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|yahoo\.com)$", ErrorMessage = "Only Gmail or Yahoo addresses are allowed.")]

        public string email { get; set; }


        [Required(ErrorMessage = "The Model field is required.")]
        public  int countryId { get; set; }


        [Required(ErrorMessage = "The Password field is required.")]
        [RegularExpression(@"^\d{6,}$", ErrorMessage = "Passwords must be at least 6 characters.")]
        public  string password { get; set; }


        [Required(ErrorMessage = "The Model field is required.")]
        public  DateTime birthDate { get; set; }


        [Required(ErrorMessage = "The Model field is required.")]
        public  bool gender { get; set; }


        [Required(ErrorMessage = "The Model field is required.")]
        public int eduLevel { get; set; }
    }
}
