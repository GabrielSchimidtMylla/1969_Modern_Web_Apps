using System;
using System.Linq;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Infra.Contexts;
using  System.Data.Entity;

namespace ModernStore.Infra.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ModernStoreDataContext _context;

        public CustomerRepository(ModernStoreDataContext context)
        {
            _context = context;
        }

        public Customer Get(Guid id)
        {
            return _context.Customer
                .Include(p => p.User)
                .FirstOrDefault(p => p.Id == id);
        }

        public bool DocumentExists(string document)
        {
            return _context.Customer
                .AsNoTracking()
                .Any(p => p.Document == document);
        }

        public void Save(Customer customer)
        {
            _context.Customer.Add(customer);
        }
    }
}
