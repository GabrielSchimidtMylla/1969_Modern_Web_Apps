using ModernStore.Domain.CommandResults;
using ModernStore.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ModernStore.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Customer Get(Guid id);

        GetCustomerCommandResult Get(string username);

        bool DocumentExists(string document);

        void Save(Customer customer);
    }
}
