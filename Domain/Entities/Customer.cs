using System;
using ModernStore.Validation;

namespace ModernStore.Domain.Entities
{
    public class Customer : Validatable
    {
        protected Customer() { }

        public Customer(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;

            if (name.Length < 3 || name.Length > 40)
                AddNotification("Nome inválido");

            if (email.Length < 3 || email.Length > 40)
                AddNotification("E-mail inválido");
        }
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
    }
}