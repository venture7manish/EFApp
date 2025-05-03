using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EFApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Simple demo user validation
        private const string DemoUsername = "admin";
        private const string DemoPassword = "password";

        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == DemoUsername && request.Password == DemoPassword)
            {
                var token = GenerateJwtToken(request.Username);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid username or password");
        }

        private string GenerateJwtToken(string username)
        {
            var secretKey = _configuration.GetValue<string>("JwtSettings:SecretKey") ?? "ThisIsASecretKeyForDevelopment";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Request DTO for login
        public class LoginRequest
        {
            public string Username { get; set; } = null!;
            public string Password { get; set; } = null!;
        }
    }
}
