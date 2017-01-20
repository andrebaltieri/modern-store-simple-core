using ModernStore.Domain.Commands;

namespace ModernStore.Domain.Handlers
{
    public interface ICommandHandler<T> where T : ICommand
    {
        void Handle(T command);
    }
}