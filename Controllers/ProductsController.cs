using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreBackend.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreBackend.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private IProductsService _service;

        public ProductsController(IProductsService service)
        {
            _service = service;

        }

        [HttpGet("/api/products")]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            try
            {
                var results = await _service.GetProducts();
                return Ok(results);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/api/products")]
        public async Task<ActionResult> AddProduct(List<Product> products)
        {
            try
            {
                var retVal = await _service.AddProduct(products);

                if (retVal > 0)
                {
                    return Ok(retVal);
                }
                else
                {
                    return BadRequest("Not able to save data");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("/api/products/{id}")]
        public async Task<ActionResult<int>> UpdateProduct(int id, Product product)
        {
            try
            {
                var retVal = await _service.UpdateProduct(id, product);
                if (retVal > 0)
                {
                    return Ok(retVal);
                }
                else
                {
                    return BadRequest("Not able to update data");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("/api/products/{id}")]
        public async Task<ActionResult<int>> DeleteProduct(int id)
        {
            try
            {
                var retVal = await _service.DeleteProduct(id);
                if (retVal > 0)
                {
                    return Ok(retVal);
                }
                else
                {
                    return BadRequest("Not able to delete data");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
