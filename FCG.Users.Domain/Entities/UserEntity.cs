using FCG.Users.Domain.Enums;
using FCG.Users.Domain.Interfaces.Repositories;

namespace FCG.Users.Domain.Entities
{
    public class UserEntity : EntityBase, IAggregateRoot
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ERole Role { get; set; } = ERole.User;

        public UserEntity() {  }

        public UserEntity(string name, string email, string password, ERole role)
        {
            Name = name;
            Email = email;
            Password = password;
            Role = role;
        }

        public void Activate()
        {
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        public void UpdateRole(ERole role)
        {
            Role = role;
        }
    }
}
