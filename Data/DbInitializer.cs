using CoreBackend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Data
{
    public class DbInitializer
    {
        public static void Initialize(ProductDBContext context)
        {
            context.Database.EnsureCreated();

            // Look for any Products.
            if (context.Products.Any())
            {
                return;   // DB has been seeded
            }

            //sample records to populate the blank table
            var products = new Product[]
            {
                new Product
                {
                    Name="Model Y",
                    Brand = "Tesla"
                },
                new Product
                {
                    Name = "Accord",
                    Brand = "Honda"
                },
                new Product
                {
                    Name = "Corolla",
                    Brand = "Toyota"
                }
            };
            foreach (Product p in products)
            {
                context.Products.Add(p);
            }
            context.SaveChanges();
        }
    }
}

/*{
	"id" : "1",
	"name": "Model y",
	"brand": "Tesla"
}

{
	"id" : 2,
	"name": "Accord",
	"brand": "Honda"
}
*/
