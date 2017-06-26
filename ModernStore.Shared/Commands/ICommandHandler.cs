using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernStore.Shared.Commands
{
    public interface ICommandHandler<in T> where T : ICommand
    {
        ICommandResult Handle(T command);
    }
}
