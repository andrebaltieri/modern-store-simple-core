using System;
using ModernStore.Domain.Enums;
using ModernStore.Validation;

namespace ModernStore.Domain.Entities
{
    public class User : Validatable
    {
        protected User() { }

        public User(string username, string password)
        {
            Id = Guid.NewGuid();
            Username = username;
            Password = password;
            Role = ERole.Admin;
        }

        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public ERole Role { get; private set; }

        public void MakeAdmin()
        {
            Role = ERole.Admin;
        }
    }
}