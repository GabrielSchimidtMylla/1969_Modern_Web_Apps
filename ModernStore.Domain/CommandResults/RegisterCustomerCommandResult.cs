using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModernStore.Shared.Commands;

namespace ModernStore.Domain.CommandResults
{
    public class RegisterCustomerCommandResult: ICommandResult
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
