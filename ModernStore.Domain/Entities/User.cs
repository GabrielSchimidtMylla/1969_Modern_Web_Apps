using System;
using FluentValidator;
using ModernStore.Shared.Entities;

namespace ModernStore.Domain.Entities
{
    public class User: Entity
    {
        public string UserName { get; private set; }

        public string Password { get; private set; }

        public  bool Active { get; private set; }

        protected User()
        {
                
        }

        public User(string userName, string password, string confirmPassword)
        {
            UserName = userName;
            Password = password;
            Active = true;

            new ValidationContract<User>(this)
                .AreEquals(p => p.Password, confirmPassword, "As senhas devem ser iguais.");
        }

        public void Activate() => Active = true;

        public void Deactivate() => Active = false;
    }
}
