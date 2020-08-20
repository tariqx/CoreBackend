using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Model
{
    public interface IProductsService
    {
        public List<Product> GetProducts();
        public Task<int> AddProduct(List<Product> productItem);
        public Task<int> UpdateProduct(int id, Product productItem);
        public Task<int> DeleteProduct(int id);

    }
}
