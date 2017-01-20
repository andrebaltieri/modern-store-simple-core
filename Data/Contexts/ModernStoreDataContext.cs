using Microsoft.EntityFrameworkCore;
using ModernStore.Domain.Entities;

namespace ModernStore.Data.Contexts
{
    public class ModernStoreDataContext : DbContext
    {
        public ModernStoreDataContext(DbContextOptions<ModernStoreDataContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<User> Users { get; set; }
    }
}