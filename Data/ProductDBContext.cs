using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBackend.Model
{
    public class ProductDBContext : DbContext
    {
        public ProductDBContext(DbContextOptions<ProductDBContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This will set the table name that we want to use rather than pluralized default used by EF 
            //Also, we will go ahead add the index to these 3 columns for better query performance in case this table gets large in future.
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("tblProduct");
                entity.HasIndex(i => new { i.ID, i.Brand, i.Name });
            });


            //seed records to tblProduct table on initial migration
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ID = 1,
                    Name = "Model Y",
                    Brand = "Tesla"
                },
                new Product
                {
                    ID = 2,
                    Name = "Accord",
                    Brand = "Honda"
                },
                new Product
                {
                    ID = 3,
                    Name = "Corolla",
                    Brand = "Toyota"
                });

        }
    }
}
