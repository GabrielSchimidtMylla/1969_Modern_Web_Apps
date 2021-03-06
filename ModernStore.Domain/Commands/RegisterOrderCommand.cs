﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernStore.Shared.Commands;

namespace ModernStore.Domain.Commands
{
    public class RegisterOrderCommand : ICommand
    {
        public Guid Customer { get; set; }

        public decimal DeliveryFee { get; set; }

        public decimal Discount { get; set; }

        public IEnumerable<RegisterOrderItemCommand> Items { get; set; }
    }
}
