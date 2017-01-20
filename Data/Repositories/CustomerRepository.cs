using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ModernStore.Data.Contexts;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;

namespace ModernStore.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ModernStoreDataContext _context;

        public CustomerRepository(ModernStoreDataContext context)
        {
            _context = context;
        }

        public void Save(Customer customer)
        {
            _context.Customers.Add(customer);
        }

        public IEnumerable<Customer> Get()
        {
            return _context.Customers.AsNoTracking();
        }
    }
}