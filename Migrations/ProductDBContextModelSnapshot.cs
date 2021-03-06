﻿// <auto-generated />
using CoreBackend.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreBackend.Migrations
{
    [DbContext(typeof(ProductDBContext))]
    partial class ProductDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoreBackend.Model.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Brand")
                        .HasColumnType("varchar(80)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(80)");

                    b.HasKey("ID");

                    b.HasIndex("ID", "Brand", "Name");

                    b.ToTable("tblProduct");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Brand = "Tesla",
                            Name = "Model Y"
                        },
                        new
                        {
                            ID = 2,
                            Brand = "Honda",
                            Name = "Accord"
                        },
                        new
                        {
                            ID = 3,
                            Brand = "Toyota",
                            Name = "Corolla"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
