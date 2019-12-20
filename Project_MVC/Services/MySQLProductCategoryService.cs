using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project_MVC.Models;
using static Project_MVC.Models.ProductCategory;

namespace Project_MVC.Services
{
    public class MySQLProductCategoryService : IProductCategoryService
    {
        private MyDbContext db = new MyDbContext();
        public bool Create(ProductCategory productCategory, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                productCategory.CreatedAt = DateTime.Now;
                productCategory.UpdatedAt = null;
                productCategory.DeletedAt = null;
                db.ProductCategories.Add(productCategory);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Delete(ProductCategory productCategory, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                productCategory.Status = ProductCategoryStatus.Deleted;
                productCategory.DeletedAt = DateTime.Now;
                db.ProductCategories.AddOrUpdate(productCategory);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public ProductCategory Detail(ProductCategory productCategory)
        {
            throw new NotImplementedException();
        }

        public bool Update(ProductCategory existProductCategory, ProductCategory productCategory, ModelStateDictionary state)
        {
            if(state.IsValid)
            {
                existProductCategory.Name = productCategory.Name;
                existProductCategory.Description = productCategory.Description;
                existProductCategory.UpdatedAt = DateTime.Now;
                db.ProductCategories.AddOrUpdate(existProductCategory);
                db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}