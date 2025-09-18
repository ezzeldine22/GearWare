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
        //Task<ValidLoginDto> Login(LoginDto loginDto);
        Task<string> Register(RegisterDto loginDto);
     
        Task<IList<Claim>> AssignRoleToUser(User User);
        
    }
}
