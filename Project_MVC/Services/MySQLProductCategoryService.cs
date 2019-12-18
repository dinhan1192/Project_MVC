using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Project_MVC.Models;
using static Project_MVC.Models.ProductCategory;

namespace Project_MVC.Services
{
    public class MySQLProductCategoryService : IProductCategoryService
    {
        private MyDbContext db = new MyDbContext();
        public ProductCategory Create(ProductCategory productCategory)
        {
            productCategory.CreatedAt = DateTime.Now;
            productCategory.UpdatedAt = null;
            productCategory.DeletedAt = null;
            db.ProductCategories.Add(productCategory);
            db.SaveChanges();
            return productCategory;
        }

        public ProductCategory Delete(ProductCategory productCategory)
        {
            productCategory.Status = ProductCategoryStatus.Deleted;
            productCategory.DeletedAt = DateTime.Now;
            db.ProductCategories.AddOrUpdate(productCategory);
            db.SaveChanges();

            return productCategory;
        }

        public ProductCategory Detail(ProductCategory productCategory)
        {
            throw new NotImplementedException();
        }

        public ProductCategory Update(ProductCategory existProductCategory, ProductCategory productCategory)
        {
            existProductCategory.Name = productCategory.Name;
            existProductCategory.Description = productCategory.Description;
            existProductCategory.UpdatedAt = DateTime.Now;
            db.ProductCategories.AddOrUpdate(existProductCategory);
            db.SaveChanges();

            return existProductCategory;
        }
    }
}