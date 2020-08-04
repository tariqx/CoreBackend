using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreBackend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private ILogger _logger;
        private IProductsService _service;

        public ProductsController(ILogger<ProductsController> logger, IProductsService service)
        {
            _logger = logger;
            _service = service;

        }

        [HttpGet("/api/products")]
        public ActionResult<List<Product>> GetProducts()
        {
            return _service.GetProducts();
        }

        [HttpPost("/api/products")]
        public ActionResult<List<Product>> AddProduct(List<Product> products)
        {
            return _service.AddProduct(products);
        }

        [HttpPut("/api/products/{id}")]
        public ActionResult<Product> UpdateProduct(int id, Product product)
        {
           var updated = _service.UpdateProduct(id, product);
            return updated;
        }

        [HttpDelete("/api/products/{id}")]
        public ActionResult<int> DeleteProduct(int id)
        {
            var idx = _service.DeleteProduct(id);
            return idx;
        }
    }
}
