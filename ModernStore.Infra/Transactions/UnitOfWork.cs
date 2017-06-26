using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernStore.Infra.Contexts;

namespace ModernStore.Infra.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ModernStoreDataContext _context;

        public UnitOfWork(ModernStoreDataContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Rollback()
        {
            //Do nothing :).
        }
    }
}
