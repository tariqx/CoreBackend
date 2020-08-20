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
        public ActionResult AddProduct(List<Product> products)
        {
            var retVal = _service.AddProduct(products);

            if (retVal.Result > 0)
            {
                return Ok(retVal.Result);
            }
            else
            {
                return BadRequest("Not able to save data");
            }

        }

        [HttpPut("/api/products/{id}")]
        public ActionResult<int> UpdateProduct(int id, Product product)
        {
           var retVal = _service.UpdateProduct(id, product);
            if (retVal.Result > 0)
            {
                return Ok(retVal.Result);
            }
            else
            {
                return BadRequest("Not able to update data");
            }
        }

        [HttpDelete("/api/products/{id}")]
        public ActionResult<int> DeleteProduct(int id)
        {
            var retVal = _service.DeleteProduct(id);
            if (retVal.Result > 0)
            {
                return Ok(retVal.Result);
            }
            else
            {
                return BadRequest("Not able to delete data");
            }
        }
    }
}
