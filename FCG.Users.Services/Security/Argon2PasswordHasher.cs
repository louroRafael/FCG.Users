using FCG.Users.Domain.Interfaces.Security;
using Isopoh.Cryptography.Argon2;

namespace FCG.Users.Services.Security
{
    public class Argon2PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return Argon2.Hash(password.Trim());
        }

        public bool Verify(string password, string hash)
        {
            return Argon2.Verify(password.Trim(), hash);
        }
    }
}
