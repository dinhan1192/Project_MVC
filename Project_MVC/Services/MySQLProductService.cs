using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Project_MVC.Models;
using static Project_MVC.Models.Product;

namespace Project_MVC.Services
{
    public class MySQLProductService : IProductService
    {
        private MyDbContext db = new MyDbContext();
        public Product Create(Product product)
        {
            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = null;
            product.DeletedAt = null;
            db.Products.Add(product);
            db.SaveChanges();
            return product;
        }

        public Product Delete(Product existProduct)
        {
            existProduct.Status = ProductStatus.Deleted;
            existProduct.DeletedAt = DateTime.Now;
            db.Products.AddOrUpdate(existProduct);
            db.SaveChanges();

            return existProduct;
        }

        public Product Detail(Product product)
        {
            throw new NotImplementedException();
        }

        public Product Update(Product existProduct, Product product)
        {
            existProduct.Name = product.Name;
            existProduct.Price = product.Price;
            existProduct.ProductCode = product.ProductCode;
            existProduct.ProductCategoryId = product.ProductCategoryId;
            existProduct.Description = product.Description;
            existProduct.UpdatedAt = DateTime.Now;
            db.Products.AddOrUpdate(existProduct);
            db.SaveChanges();

            return existProduct;
        }
    }
}