namespace FCG.Users.Domain.DTOs.Responses
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime? ExpireIn { get; set; }

        public LoginResponse (string token, DateTime expireIn)
        {
            Token = token;
            ExpireIn = expireIn;
        }
    }
}
