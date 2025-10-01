using BLL.DTOs.AccountDTOs;
using BLL.DTOs.ProductDtos;
using BLL.Exceptions;
using BLL.Managers.AccountManager;
using BLL.Services.ProductServices;
using DAL.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AccountManager
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Logger<AccountController> _logger;

        public AccountController(IAccountManager accountManager , UserManager<User> userManager, SignInManager<User> signInManager,ILogger<AccountController> _logger)
        {
            _accountManager = accountManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = _logger;
        }


        [HttpGet("external-login")]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
           
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });

            var properties = new AuthenticationProperties
            {
                RedirectUri = redirectUrl,  
                IsPersistent = true,  
                ExpiresUtc = DateTime.UtcNow.AddMonths(1)  
            }; 
            return Challenge(properties, provider);
        }

        [HttpGet("external-login-callback")]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
       
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);

            if (result?.Principal == null)
            {
                _logger.LogWarning("External login failed: no principal returned.");
                return RedirectToAction("Login");
            }

            var externalUser = result.Principal;
            var email = externalUser.FindFirstValue(ClaimTypes.Email);
            var provider = result.Properties?.Items[".AuthScheme"];
            var providerKey = externalUser.FindFirstValue(ClaimTypes.NameIdentifier);
            var gender = externalUser.FindFirstValue(ClaimTypes.Gender);
        
            if (string.IsNullOrEmpty(provider) || string.IsNullOrEmpty(providerKey))
            {
                _logger.LogError("External login failed: provider or provider key missing.");
                return BadRequest("Login failed. Try another provider.");
            }

            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("External login provider did not supply an email.");
                return BadRequest("Email not provided by external provider.");
            }

    
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var fullName = email.Split('@')[0]; 
                user = new User
                {
                    UserName = email,
                    Email = email,
                    Name = fullName,
                    gender = gender
            
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    _logger.LogError("Failed to create user: {Errors}", string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    return BadRequest("Could not create user account.");
                }
            }

          
            var externalLoginInfo = new UserLoginInfo(provider, providerKey, provider);
            var addLoginResult = await _userManager.AddLoginAsync(user, externalLoginInfo);

            if (!addLoginResult.Succeeded)
            {
                _logger.LogWarning("Failed to add external login for user {Email}.", email);
            }

        
            var userClaims = await _userManager.GetClaimsAsync(user);
            var token = _accountManager.GenerateToken(userClaims);

          
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

     
            return Ok(new { Token = token, UserId = user.Id });
        }
        
        
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _accountManager.Register(registerDto);
                return Ok();
            }
            catch (CustomException ex)
            {
                return BadRequest(new
                {
                    ex.Errors
                });
            }
        }



        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<ValidloginDto>> Login(LoginDto LoginDto)
        {
            try
            {
               var LoginResults = await _accountManager.Login(LoginDto);
                return Ok(LoginResults);
            }
            catch (CustomException ex)
            {
                return BadRequest(new
                {
                    ex.Errors
                });
            }
        }
    }
}
