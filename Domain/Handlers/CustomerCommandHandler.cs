using ModernStore.Domain.Commands;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Validation;

namespace ModernStore.Domain.Handlers
{
    public class CustomerCommandHandler :
    Validatable,
        ICommandHandler<RegisterCustomerCommand>
    {
        private readonly ICustomerRepository _repository;

        public CustomerCommandHandler(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public void Handle(RegisterCustomerCommand command)
        {
            var customer = new Customer(command.Name, command.Email);

            if (customer.IsValid())
                _repository.Save(customer);

            AddNotification(customer.Notifications);
        }
    }
}