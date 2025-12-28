using FCG.Users.Domain.Entities;

namespace FCG.Users.Domain.Interfaces.Security
{
    public interface ITokenService
    {
        string GenerateToken(UserEntity user);
        DateTime GetExpirationDate(string token);
    }
}
