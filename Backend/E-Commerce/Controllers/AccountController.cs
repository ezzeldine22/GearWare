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
    // Test //
    // Test2 //
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Logger<AccountController> _logger;

        public AccountController(IAccountManager accountManager , UserManager<User> userManager, SignInManager<User> signInManager, Logger<AccountController> logger)
        {
            _accountManager = accountManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
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
            var provider = result.Properties?.Items[".AuthScheme"];

            var user = await _accountManager.ExternalLoginCallBackAsync(result.Principal, provider);
            var userClaims = await _userManager.GetClaimsAsync(user);
           var token = _accountManager.GenerateToken(userClaims);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Ok(new { Token = token, UserName = user.Name , UserId=user.Id});
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
