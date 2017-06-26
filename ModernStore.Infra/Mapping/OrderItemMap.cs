using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernStore.Domain.Entities;

namespace ModernStore.Infra.Mapping
{
    public class OrderItemMap : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemMap()
        {
            HasKey(p => p.Id);

            Property(p => p.Price).HasColumnType("money");

            Property(p => p.Quantity);

            HasRequired(p => p.Product);
        }
    }
}
