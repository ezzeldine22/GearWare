using BLL.DTOs.AccountDTOs;
using BLL.DTOs.ProductDtos;
using BLL.Exceptions;
using BLL.Managers.AccountManager;
using BLL.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AccountManager
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _accountManager;

        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
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
