using AdvertisingAds.Core.Interfaces;
using AdvertisingAds.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AdvertisingAds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            var result = await _userService.Signup(user);
            if (result)
            {
                return Ok(true);
            }
            return BadRequest("User already exists.");
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Signin([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.password) || string.IsNullOrEmpty(user.Email))
            {
                return BadRequest("User credentials are required.");
            }

            var authenticatedUser = await _userService.Signin(user);
            if (authenticatedUser != null)
            {
                return Ok(authenticatedUser); // You may want to return a token instead
            }
            return Unauthorized("Invalid credentials.");
        }
    }
}
