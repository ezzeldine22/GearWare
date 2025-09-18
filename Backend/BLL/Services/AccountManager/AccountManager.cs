using BLL.DTOs.AccountDTOs;
using BLL.Exceptions;
using DAL.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Managers.AccountManager
{
    public class AccountManager : IAccountManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountManager(UserManager<User> userManager,
                              IConfiguration configuration,
                              RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<IList<Claim>> AssignRoleToUser(User User)
        {

            if (User != null)
            {
                var result = await _userManager.AddToRoleAsync(User, "User");

                if (result.Succeeded)
                {
                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                    claims.Add(new Claim(ClaimTypes.Name, User.UserName));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, User.Id));


                    await _userManager.AddClaimsAsync(User, claims);
                }
               
            }
            var claim = await _userManager.GetClaimsAsync(User);
            return claim;
        }


        //public async Task<ValidLoginDto> Login(LoginDto logInDto)
        //{
        //    var user = await _userManager.FindByEmailAsync(logInDto.emailOrPhone);

        //    if (user == null)
        //    {
        //        user = _userManager.Users.FirstOrDefault(a => a.PhoneNumber == logInDto.emailOrPhone); ;
        //        if (user == null)
        //        {
        //            throw new CustomException(new List<string> { "لا يوجد حساب مرتبط بالبريد الإلكتروني" });
        //        }
        //    }

        //    var check = await _userManager.CheckPasswordAsync(user, logInDto.password);

        //    if (check == false)
        //    {
        //        throw new CustomException(new List<string> { "برجاء التأكد من البيانات والمحاولة مرة أخرى" });
        //    }

        //    var role = await _userManager.GetRolesAsync(user);
        //    var claims = await _userManager.GetClaimsAsync(user);
        //    return new ValidLoginDto
        //    {
        //        token = GenerateToken(claims),
        //        email = user.Email,
        //        firstName = user.Fname,
        //        roles = role,
        //        isVerified = false
        //    };
        //}

        public async Task<string> Register(RegisterDto registerDto)
        {
            var CheckEmailRedundncy = await _userManager.Users.AnyAsync(u => u.Email == registerDto.email);
           
            if (CheckEmailRedundncy)
            {
                throw new CustomException(new List<string> { "The Email Already Exists !!!" });
            }

            var CheckPhoneRedundncy = await _userManager.Users.AnyAsync(u => u.PhoneNumber == registerDto.phone);

            if (CheckPhoneRedundncy)
            {
                throw new CustomException(new List<string> { "The Phone Already Exists !!!" });
            }

      
            
            var newUser = new User
            {
                Name = registerDto.Name,
                Email = registerDto.email,
                PhoneNumber = registerDto.phone,
                gender = registerDto.gender,
                UserName = registerDto.Name+"123",
            };

             var hashedPass = await _userManager.CreateAsync(newUser,registerDto.password);


             if (hashedPass.Succeeded)
             {
                return "No Issues Found";
             }
             var Errors = hashedPass.Errors.ToString();
             
             return Errors;

        }

        
        private string GenerateToken(IList<Claim> claims)
        {
            var secretKey = _configuration.GetSection("SecretKey").Value;

            var secretKeyByte = Encoding.UTF8.GetBytes(secretKey);

            SecurityKey securityKey = new SymmetricSecurityKey(secretKeyByte);

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expire = DateTime.UtcNow.AddDays(30);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(claims: claims, expires: expire, signingCredentials: signingCredentials);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);
            return token;
        }
    }
}