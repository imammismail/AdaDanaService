using AdaDanaService.Data;
using AdaDanaService.Dtos;
using AdaDanaService.Models;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "User")]
        [HttpPut]
        public async Task<string> PassowordUser(UpdatePassword updatePassword)
        {
            try
            {
                await _accountService.UpdatePasswordUser(updatePassword);
                return "Password updated successfully";
            }
            catch (ArgumentException ex)
            {
                return $"Error: {ex.Message}";
            }
            catch (Exception)
            {
                return "Error updating password";
            }
        }
    }
}
