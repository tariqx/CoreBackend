using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Model
{
public interface IProductsService
{
    public List<Product> GetProducts();
    public List<Product> AddProduct(List<Product> productItem);
    public Product UpdateProduct(int id, Product productItem);
    public int DeleteProduct(int id);

}
}
