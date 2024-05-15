using AtmiraPayNet.Server.Models;
using AtmiraPayNet.Server.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AtmiraPayNet.Server.Services
{
    public class TokenService(IConfiguration config, IHttpContextAccessor httpContextAccessor) : ITokenService
    {
        private readonly string _secretKey = config.GetSection("settings").GetSection("secretKey").ToString()!;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Name, user.UserName!),
                    new("fullName", user.FullName),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string? GetCurrentUserId()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext!.Request.Headers.Authorization;

            string? token = string.IsNullOrEmpty(authorizationHeader)
                ? null
                : authorizationHeader.ToString().Split(" ")[1];

            Claim? userIdClaim = string.IsNullOrEmpty(token)
                ? null
                : new JwtSecurityTokenHandler().ReadJwtToken(token).Claims.FirstOrDefault(c => c.Type == "nameid");

            return userIdClaim?.Value;
        }
    }
}
