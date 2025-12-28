using FCG.Users.Domain.Validators;
using FluentValidation;

namespace FCG.Users.Domain.DTOs.Requests;

public class LoginRequest
{
    public string Email { get; private set; }
    public string Password { get; private set; }

    public LoginRequest (string email, string password)
    {
        Email = email;
        Password = password;
    }
}
