using AdvertisingAds.Core.Interfaces;
using AdvertisingAds.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisingAds.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly string _secretKey;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _secretKey = configuration["JwtSettings:SecretKey"];
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            var authenticatedUser = await _userService.Signup(user);
            if (authenticatedUser != null)
            {
                var token = GenerateJwtToken(authenticatedUser);
                return Ok(new { user = authenticatedUser, token = token });
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
                var token = GenerateJwtToken(authenticatedUser);
                return Ok(new { user = authenticatedUser, token = token });
            }
            return Unauthorized("Invalid credentials.");
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
