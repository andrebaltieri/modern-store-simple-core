using System.Collections.Generic;
using ModernStore.Domain.Entities;

namespace ModernStore.Domain.Repositories
{
    public interface ICustomerRepository
    {
        IEnumerable<Customer> Get();
        void Save(Customer customer);
    }
}