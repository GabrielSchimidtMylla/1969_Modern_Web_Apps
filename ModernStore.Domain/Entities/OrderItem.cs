﻿using FluentValidator;
using ModernStore.Shared.Entities;

namespace ModernStore.Domain.Entities
{
    public class OrderItem: Entity
    {
        public Product Product { get; private set; }

        public int Quantity{ get; private set; }

        public decimal Price { get; private set; }

        protected OrderItem()
        {
            
        }

        public OrderItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            Price = Product.Price;

            new ValidationContract<OrderItem>(this)
                .IsGreaterThan(x => x.Quantity, 1)
                .IsGreaterThan(x => x.Product.QuantityOnHand, Quantity + 1,
                    $"Não temos tantos {Product.Title} em estoque.");

            Product.DecreaseQuantity(quantity);
        }

        public decimal Total() => Price * Quantity;
    }
}
