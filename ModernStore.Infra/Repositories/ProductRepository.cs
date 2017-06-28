using System;
using System.Collections.Generic;
using System.Linq;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Infra.Contexts;

namespace ModernStore.Infra.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ModernStoreDataContext _context;

        public ProductRepository(ModernStoreDataContext context)
        {
            _context = context;
        }

        public Product Get(Guid id)
        {
            return _context.Product
                .AsNoTracking()
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> Get()
        {
            return _context.Product
                .AsNoTracking()
                .AsEnumerable();
        }
    }
}
