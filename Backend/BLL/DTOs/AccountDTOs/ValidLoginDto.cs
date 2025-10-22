using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs.AccountDTOs
{
    public class ValidloginDto
    {
        public string Id { get; set; }
        public string token { get; set; }
        public string email { get; set; }
        public string Name { get; set; }
        public IList<string> roles { get; set; }
        //public bool isVerified { get; set; }
    }
}
