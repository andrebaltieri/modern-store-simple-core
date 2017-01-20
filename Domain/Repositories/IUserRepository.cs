using ModernStore.Domain.Entities;

namespace ModernStore.Domain.Repositories
{
    public interface IUserRepository
    {
        User Get(string username, string password);
        void Save(User user);
    }
}