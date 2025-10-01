using BLL.DTOs.AccountDTOs;
using DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers.AccountManager
{
    public interface IAccountManager
    {
        Task<ValidloginDto> Login(LoginDto loginDto);
        string GenerateToken(IList<Claim> claims);
        Task<User> ExternalLoginCallBackAsync(ClaimsPrincipal claimsPrincipal , string provider);
        Task<string> Register(RegisterDto RegisterDto);
  
    }
}
