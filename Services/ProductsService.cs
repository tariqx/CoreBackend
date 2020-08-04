using CoreBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Services
{
    public class ProductsService : IProductsService
    {
        private List<Product> _productItems;

        public ProductsService()
        {
            _productItems = new List<Product>();
        }

        List<Product> IProductsService.AddProduct(List<Product> productItem)
        {
            //adds new project into the list collection 
            _productItems.AddRange(productItem);
            //for now return back the original what was passed in
            return productItem;
        }

        int IProductsService.DeleteProduct(int id)
        {
            //find the product(s) based on the ID and remove it
            return _productItems.RemoveAll(x => x.ID == id); ;
        }

        List<Product> IProductsService.GetProducts()
        {
            //return all products
            return _productItems;
        }

        Product IProductsService.UpdateProduct(int id, Product productItem)
        {
            //search for the product by ID, when found update that product object in collection and finally return that single updated one. 
            var idx = _productItems.FindIndex(p => p.ID == id);
            _productItems[idx] = productItem;

            return _productItems[idx];
        }
    }
}
