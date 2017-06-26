using FluentValidator;
using ModernStore.Domain.CommandResults;
using ModernStore.Domain.Commands;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Domain.Services;
using ModernStore.Domain.ValueObjects;
using ModernStore.Shared.Commands;

namespace ModernStore.Domain.CommandHandlers
{
    public class CustomerCommandHadler : Notifiable , ICommandHandler<RegisterCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;

        public CustomerCommandHadler(ICustomerRepository customerRepository, IEmailService emailService)
        {
            _customerRepository = customerRepository;
            _emailService = emailService;
        }

        public ICommandResult Handle(RegisterCustomerCommand command)
        {
            var exists = _customerRepository.DocumentExists(command.Document);

            if (exists)
            {
                AddNotification("Document", "Este CPF já esta em uso");
                return null;
            }

            var name = new Name(command.FirstName, command.LastName);
            var user = new User(command.Username, command.Password, command.ConfirmPassword);
            var customer = new Customer(name, command.BirthDate, command.Email, command.Document, user);

            AddNotifications(name.Notifications);
            AddNotifications(user.Notifications);

            if (IsValid())
                _customerRepository.Save(customer);

            //Envia e-mail
            _emailService.Send(customer.Name.ToString(), customer.Email, "Bem-vindo", "Boas vindas ao meu sistema");

            //Retorna alguma coisa
            return new RegisterCustomerCommandResult()
            {
                Id = customer.Id,
                Name =  customer.Name.ToString()
            };
        }
    }
}
