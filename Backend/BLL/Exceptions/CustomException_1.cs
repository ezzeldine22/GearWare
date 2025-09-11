using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Exceptions
{
    public class CustomException : Exception
    {
        public List<string> Errors { get; }

        public CustomException(List<string> errors)
        {
            Errors = errors;
        }
    }
}
