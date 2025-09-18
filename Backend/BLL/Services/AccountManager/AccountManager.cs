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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountManager(UserManager<ApplicationUser> userManager,
                              IConfiguration configuration,
                              RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public async Task<IList<Claim>> AssignRoleToUser(ApplicationUser User)
        {
            var user = await _userManager.FindByIdAsync(User.Id);
            //var role = await _roleManager.FindByIdAsync(assignRole.RoleId);

            if(user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, "User");

                if (result.Succeeded)
                {
                    List<Claim> claims = new List<Claim>();

                    claims.Add(new Claim(ClaimTypes.Role, "User"));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                    await _userManager.AddClaimsAsync(user, claims);
                }
                //return "Save Fail";
            }
            var claim = await _userManager.GetClaimsAsync(user);
            return claim;
        }


        public async Task<ValidLoginDto> Login(LoginDto logInDto)
        {
            var user = await _userManager.FindByEmailAsync(logInDto.emailOrPhone);

            if (user == null)
            {
                user = _userManager.Users.FirstOrDefault(a => a.PhoneNumber == logInDto.emailOrPhone); ;
                if (user == null)
                {
                    throw new CustomException(new List<string> { "لا يوجد حساب مرتبط بالبريد الإلكتروني" });
                }
            }

            var check = await _userManager.CheckPasswordAsync(user, logInDto.password);

            if (check == false)
            {
                throw new CustomException(new List<string> { "برجاء التأكد من البيانات والمحاولة مرة أخرى" });
            }

            var role = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            return new ValidLoginDto
            {
                token = GenerateToken(claims),
                email = user.Email,
                firstName = user.Fname,
                roles = role,
                isVerified = false
            };
        }

        public async Task<string> Register(RegisterDto registerDto)
        {
            var checkphone = await _userManager.Users.FirstOrDefaultAsync(a => a.PhoneNumber == registerDto.phone);

            if (checkphone != null)
            {
                throw new CustomException(new List<string> { "رقم الهاتف مسجل بالفعل لحساب آخر!" });
            }

            var checkEmail = await _userManager.FindByEmailAsync(registerDto.email);

            if(checkEmail != null)
            {
                throw new CustomException(new List<string> { $"Email '{registerDto.email}' is already taken." });
            }
            //if(registerDto.password.Length < 6)
            //{
            //    throw new CustomException(new List<string> { "Passwords must be at least 6 characters." });
            //}

            ApplicationUser user = new ApplicationUser
            {
                Fname = registerDto.firstName,
                Lname = registerDto.lastName,
                UserName = registerDto.firstName + "_" + registerDto.lastName,
                Gender = registerDto.gender,
                CountryId = registerDto.countryId,
                Email = registerDto.email,
                PhoneNumber = registerDto.phone,
                Birthdate = registerDto.birthDate,
                EduLevel = registerDto.eduLevel,
            };
            
            var result = await _userManager.CreateAsync(user, registerDto.password);

            //if (!result.Succeeded)
            //{
            //    var errors = result.Errors.Select(e => e.Description).ToList();
            //    throw new CustomException(errors);
            //}

            if (result.Succeeded)
            {
                var createdUser = await _userManager.FindByEmailAsync(registerDto.email);

                var claim = await AssignRoleToUser(createdUser);
                
                return GenerateToken(claim);
            }
            return null;
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