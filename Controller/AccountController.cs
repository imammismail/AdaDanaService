using AdaDanaService.Data;
using AdaDanaService.Dtos;
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
        public IActionResult Register(RegisterUserDto registerUser)
        {
            var result = _accountService.Register(registerUser);

            if (result)
            {
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest("Failed to register user");
            }
        }
    }
}
