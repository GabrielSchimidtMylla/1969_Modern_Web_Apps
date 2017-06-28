using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModernStore.Domain.CommandHandlers;
using ModernStore.Domain.Commands;
using ModernStore.Domain.Repositories;
using ModernStore.Infra.Transactions;

namespace ModelStore.Api.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly CustomerCommandHadler _hadler;

        public CustomerController(IUnitOfWork unitOfWork, CustomerCommandHadler hadler)
            :base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _hadler = hadler;
        }

        [HttpPost]
        [Route("v1/customers")]
        public async Task<IActionResult> Post([FromBody] RegisterCustomerCommand command)
        {
            var result = _hadler.Handle(command);
            return await Response(result, _hadler.Notifications);
        }
    }
}
