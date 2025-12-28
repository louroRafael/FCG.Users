using FCG.Users.Domain.Enums;
using FluentValidation;

namespace FCG.Users.Domain.DTOs.Requests;

public class CreateAdminRequest
{
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Password { get; private set; }
    public ERole Role { get; set; }

    public CreateAdminRequest(string name, string email, string password, ERole role = ERole.User)
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }
}