using BLL.DTOs.AccountDTOs;
using BLL.DTOs.ProductDtos;
using BLL.Exceptions;
using BLL.Managers.AccountManager;
using BLL.Services.ProductServices;
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
    public class RegisterController : ControllerBase
    {
        private readonly IAccountManager _accountManager;

        public RegisterController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }
        [HttpPost("Register")]
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
    }
}
