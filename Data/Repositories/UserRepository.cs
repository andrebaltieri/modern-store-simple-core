using System.Linq;
using ModernStore.Data.Contexts;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;

namespace ModernStore.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ModernStoreDataContext _context;

        public UserRepository(ModernStoreDataContext context)
        {
            _context = context;
        }

        public User Get(string username, string password) => _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);

        public void Save(User user) => _context.Users.Add(user);
    }
}