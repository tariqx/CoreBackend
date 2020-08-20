using CoreBackend.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Services
{
    public class ProductsService : IProductsService
    {
        private ProductDBContext _dbctx;
        private ILogger _logger;


        public ProductsService(ProductDBContext dbctx, ILogger<ProductsService> logger)
        {
            _dbctx = dbctx;
            _logger = logger;
        }

        async Task<int> IProductsService.AddProduct(List<Product> productItems)
        {

            try
            {
                _dbctx.Products.AddRange(productItems);
                
                return await _dbctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                return 0;
            }
        }

        async Task<int> IProductsService.DeleteProduct(int id)
        {

            try
            {            
                //find the product based on the ID
                var p = _dbctx.Products.SingleOrDefault(p => p.ID == id);
                _dbctx.Products.Remove(p); //remove it from the context 
                return await _dbctx.SaveChangesAsync(); //save changes to actaully remove it from the database
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                return 0;
            }
        }

        List<Product> IProductsService.GetProducts()
        {
            //return all products list from database
            return _dbctx.Products.ToList();
        }

        async Task<int> IProductsService.UpdateProduct(int id, Product productItem)
        {
            try
            {
                //find the specific product item we are looking for 
                var p = _dbctx.Products.SingleOrDefault(p => p.ID == id);
                _dbctx.Entry(p).CurrentValues.SetValues(productItem);

                return await _dbctx.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.InnerException.Message);
                return 0;
            }
        }
    }
}
