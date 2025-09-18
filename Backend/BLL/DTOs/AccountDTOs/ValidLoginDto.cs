using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.AccountDTOs
{
    public class ValidLoginDto
    {
        public string token { get; set; }
        public  string email { get; set; }
        public  string firstName { get; set; }
        public  IList<string> roles { get; set; }
        public bool isVerified { get; set; }
    }
}
