using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModernStore.Infra.Transactions;

namespace ModelStore.Api.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
