using Azure;
using EcommerceProjectBLL.Dto.AccountDto;
using EcommerceProjectBLL.HandlerResponse;
using EcommerceProjectBLL.Manager.AccountManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProjectAPI.Controllers.AccountController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountManager _accountManager;
        

        public AccountController(IAccountManager accountManager)
        {
            _accountManager=accountManager;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(a => a.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new { Message = "Failed to Registered ..", Errors = errors });
            }

            var result = await _accountManager.Register(registerDto);
            if (result.IsAuthenticated==false)
            {
                return Unauthorized(new { Error = result.Errors });
            }
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult>Login(LoginDto loginDto)
        {
            //make sure the data is true if not
            if(!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(a => a.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new {Message="Failed to login..",Errors =errors});
            }

            var result = await _accountManager.Login(loginDto);
            if(!result.IsAuthenticated)
            {
                return Unauthorized(new { Message = result.Message });
            }
            return Ok(result);
        }
        [HttpPost("VerifyEmail")]
        public async Task<ActionResult<AuthModel>> VerifyEmail(VerifyEmailDto verifyEmailDto)
        {
            var result = await _accountManager.VerifyEmail(verifyEmailDto);
            if (result.Errors != null && result.Errors.Any())
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("ForgotPassword")]
        public async Task<ActionResult<AuthModel>> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var result = await _accountManager.ForgotPasswordAsync(forgotPasswordDto);
            if(result.Errors!=null&&result.Errors.Any())
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<AuthModel>> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await _accountManager.ResetPasswordAsync(resetPasswordDto);
            if (result.Errors!=null&&result.Errors.Any())
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


    }
}
