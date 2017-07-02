using System;
using System.Linq;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Infra.Contexts;
using  System.Data.Entity;
using ModernStore.Domain.CommandResults;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using ModernStore.Shared;

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

        public Customer GetByUserName(string username)
        {
            return _context.Customer
                .Include(p => p.User)
                .AsNoTracking()
                .FirstOrDefault(p => p.User.UserName == username);
        }

        public GetCustomerCommandResult Get(string username)
        {
            using (var conn = new SqlConnection(Runtime.ConnectionString))
            {
                conn.Open();
                return conn.QueryFirstOrDefault(
                    "SELECT * FROM GetCustomerInfoView WHERE Active = 1 and Username = @username",
                    new
                    {
                        username = username
                    });
            }
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
