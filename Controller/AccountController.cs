using AdaDanaService.Data;
using AdaDanaService.Dtos;
using AdaDanaService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdaDanaService.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserDto registerUser)
        {
            var result = await _accountService.Register(registerUser);

            if (result)
            {
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest("Failed to register user");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserToken>> Login(LoginDto user)
        {
            var result = await _accountService.Login(user);
            if (result != null)
            {
                return result;
            }
            return BadRequest(new UserToken { Message = "Invalid username or password" });
        }
    }
}
