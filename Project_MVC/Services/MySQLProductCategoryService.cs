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
    public class MySQLProductCategoryService : ICRUDService<ProductCategory>
    {
        private MyDbContext db = new MyDbContext();
        public bool Create(ProductCategory item, ModelStateDictionary state)
        {
            ValidateCode(item, state);
            if (state.IsValid)
            {
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = null;
                item.DeletedAt = null;
                item.Status = ProductCategoryStatus.NotDeleted;
                db.ProductCategories.Add(item);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Delete(ProductCategory item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                item.Status = ProductCategoryStatus.Deleted;
                item.DeletedAt = DateTime.Now;
                db.ProductCategories.AddOrUpdate(item);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public ProductCategory Detail(ProductCategory productCategory)
        {
            throw new NotImplementedException();
        }

        public bool Update(ProductCategory existItem, ProductCategory item, ModelStateDictionary state)
        {
            if(state.IsValid)
            {
                existItem.Name = item.Name;
                existItem.Description = item.Description;
                existItem.UpdatedAt = DateTime.Now;
                db.ProductCategories.AddOrUpdate(existItem);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public void ValidateCategory(ProductCategory item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void ValidateCode(ProductCategory item, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(item.Code))
            {
                state.AddModelError("Code", "Product Category Code is required.");
            }
            var list = db.ProductCategories.Where(s => s.Code.Contains(item.Code)).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("Code", "Product Category Code already exist.");
            }
        }
    }
}