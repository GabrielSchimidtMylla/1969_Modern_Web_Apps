using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidator;
using Microsoft.AspNetCore.Mvc;
using ModernStore.Infra.Transactions;

namespace ModelStore.Api.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Response(object result, IEnumerable<Notification> notification)
        {
            if (!notification.Any())
            {
                try
                {
                    _unitOfWork.Commit();
                    return Ok(new
                    {
                        success = true,
                        data = result
                    });
                }
                catch (Exception)
                {
                    return BadRequest(new
                    {
                        success = false,
                        errors = new string[] { "Ocorreu uma falha interna no servidor." }
                    });
                }
            }

            return BadRequest(new
            {
                success = false,
                errors = notification.ToArray()
            });
        }
    }
}
