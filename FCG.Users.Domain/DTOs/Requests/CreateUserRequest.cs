using FluentValidation;

namespace FCG.Users.Domain.DTOs.Requests;

public class CreateUserRequest
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }

    public CreateUserRequest(string name, string email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}
