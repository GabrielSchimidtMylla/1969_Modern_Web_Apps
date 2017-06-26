using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernStore.Domain.Entities;

namespace ModernStore.Infra.Mapping
{
    public class UserMap: EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            HasKey(p => p.Id);
            Property(p => p.Active);
            Property(p => p.Password);
            Property(p => p.UserName);
        }
    }
}
