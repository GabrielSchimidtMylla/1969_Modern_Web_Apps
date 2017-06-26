using System.Data.Entity.ModelConfiguration;
using ModernStore.Domain.Entities;

namespace ModernStore.Infra.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            HasKey(p => p.Id);

            Property(p => p.Number).IsRequired();

            Property(p => p.CreateDate);

            Property(p => p.DeliveryFee).HasColumnType("money");

            Property(p => p.Discount).HasColumnType("money");

            Property(p => p.Status).IsRequired();

            HasMany(p => p.Items);

            HasRequired(p => p.Customer);
        }
    }
}
