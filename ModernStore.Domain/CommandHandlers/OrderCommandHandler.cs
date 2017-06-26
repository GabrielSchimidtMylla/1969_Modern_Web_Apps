using System.Linq;
using FluentValidator;
using ModernStore.Domain.CommandResults;
using ModernStore.Domain.Commands;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Shared.Commands;

namespace ModernStore.Domain.CommandHandlers
{
    public class OrderHandler : Notifiable, ICommandHandler<RegisterOrderCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;

        public OrderHandler(ICustomerRepository customerRepository, IProductRepository productRepository, IOrderRepository orderRepository)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
        }

        public ICommandResult Handle(RegisterOrderCommand command)
        {
            //Busca os dados dos clientes.
            var customer = _customerRepository.Get(command.Customer);

            //Gera um novo pedido.
            var order = new Order(customer, command.DeliveryFee, command.Discount);

            command.Items.ToList().ForEach(p =>
            {
                var product = _productRepository.Get((p.Product));
                order.AddItem(new OrderItem(product, p.Quantity));
            });

            //Adiciona as notificações do pedido no handler.
            AddNotifications(order.Notifications);

            if (order.IsValid())
                _orderRepository.Save(order);

            return new RegisterOrderCommandResult
            {
                Number = order.Number
            };
        }
    }
}
