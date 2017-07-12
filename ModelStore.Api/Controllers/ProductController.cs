using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModernStore.Domain.Repositories;

namespace ModelStore.Api.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        [Route("v1/products")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(_productRepository.Get());
        }
    }
}
