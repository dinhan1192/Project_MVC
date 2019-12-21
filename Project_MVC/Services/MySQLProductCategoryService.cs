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
            ValidateCode(productCategory, state);
            if (state.IsValid)
            {
                productCategory.CreatedAt = DateTime.Now;
                productCategory.UpdatedAt = null;
                productCategory.DeletedAt = null;
                productCategory.Status = ProductCategoryStatus.NotDeleted;
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

        public void ValidateCode(ProductCategory productCategory, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(productCategory.Code))
            {
                state.AddModelError("Code", "Product Category Code is required.");
            }
            var list = db.ProductCategories.Where(s => s.Code.Contains(productCategory.Code)).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("Code", "Product Category Code already exist.");
            }
        }
    }
}