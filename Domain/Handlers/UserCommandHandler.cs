using ModernStore.Domain.Commands;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Validation;

namespace ModernStore.Domain.Handlers
{
    public class UserCommandHandler :
    Validatable,
        ICommandHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _repository;

        public UserCommandHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public void Handle(RegisterUserCommand command)
        {
            var user = new User(command.Username, command.Password);

            if (user.IsValid())
                _repository.Save(user);

            AddNotification(user.Notifications);
        }
    }
}