using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernStore.Domain.Entities;

namespace ModernStore.Infra.Mapping
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            HasKey(p => p.Id);

            Property(p => p.BirthDate);

            Property(p => p.Email)
                .IsRequired();

            Property(p => p.Document)
                .IsRequired();

            Property(p => p.Name.FirstName)
                .IsRequired();

            Property(p => p.Name.LastName)
                .IsRequired();

            HasRequired(p => p.User);
        }
    }
}
