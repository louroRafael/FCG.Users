using FCG.Users.Domain.Entities;
using FCG.Users.Domain.Interfaces.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FCG.Users.Services.Auth
{
    public class TokenService(IConfiguration configuration) : ITokenService
    {
        public string GenerateToken(UserEntity user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var issuer = configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("JWT Issuer not configured");
            var audience = configuration["Jwt:Audience"] ?? throw new InvalidOperationException("JWT Audience not configured");
            var keyString = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured");

            var key = Encoding.UTF8.GetBytes(keyString);

            var creds = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public DateTime GetExpirationDate(string token)
        {
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwtToken.ValidTo;
        }
    }
}
