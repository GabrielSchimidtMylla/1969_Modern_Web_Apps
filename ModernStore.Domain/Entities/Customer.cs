using System;
using System.Collections.Generic;
using FluentValidator;
using ModernStore.Domain.ValueObjects;
using ModernStore.Shared.Commands;
using ModernStore.Shared.Entities;

namespace ModernStore.Domain.Entities
{
    public class Customer : Entity, ICommand
    {
        public Name Name { get; private set; }

        public DateTime BirthDate { get; private set; }

        public string Email { get; private set; }

        public string Document { get; private set; }

        public User User { get; private set; }

        protected Customer()
        {
        }

        public Customer(Name name, DateTime birthDate, string email, string document, User user)
        {
            Name = name;
            BirthDate = birthDate;
            Email = email;
            Document = document;
            User = user;

            new ValidationContract<Customer>(this)
                .IsEmail(x => x.Email,"E-mail inválido")
                .IsRequired(x => x.Document);

            AddNotifications(Name.Notifications);
        }

    }
}
