using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace EFApi.Middleware
{
    public class JwtMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _logger;
        private readonly string _secretKey;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> logger, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _secretKey = configuration["Jwt:Key"] ?? "";
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key =Encoding.ASCII.GetBytes(_secretKey);

                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false, // you might want to validate issuer and audience in real apps
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    // If needed, attach user info to context here
                    _logger.LogInformation("JWT token is valid.");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"JWT validation failed: {ex.Message}");
                    // Optionally, reject request if token is invalid:
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Unauthorized: Invalid token");
                    return;
                }
            }
            else
            {
                _logger.LogInformation("No JWT token found in request.");
            }

            await _next(context);
        }
    }
}
