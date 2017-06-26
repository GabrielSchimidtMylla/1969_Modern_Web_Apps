using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernStore.Domain.Entities;

namespace ModernStore.Infra.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            HasKey(p => p.Id);

            Property(p => p.Image)
                .IsRequired()
                .HasMaxLength(1024);

            Property(p => p.Price).HasColumnType("money");

            Property(p => p.QuantityOnHand);

            Property(p => p.Title);

        }
    }
}
