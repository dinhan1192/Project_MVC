using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using static Project_MVC.Models.ProductCategory;

namespace Project_MVC.Services
{
    public class MySQLProductCategoryService : ICRUDService<ProductCategory>
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private IUserService userService;

        public MySQLProductCategoryService()
        {
            userService = new UserService();
        }

        public bool Create(ProductCategory item, ModelStateDictionary state)
        {
            Validate(item, state);
            if (state.IsValid)
            {
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = null;
                item.DeletedAt = null;
                item.CreatedBy = userService.GetCurrentUserName();
                item.Status = ProductCategoryStatus.NotDeleted;
                DbContext.ProductCategories.Add(item);
                DbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool CreateWithImage(ProductCategory item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ProductCategory item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                item.Status = ProductCategoryStatus.Deleted;
                item.DeletedAt = DateTime.Now;
                item.DeletedBy = userService.GetCurrentUserName();
                DbContext.ProductCategories.AddOrUpdate(item);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public ProductCategory Detail(string id)
        {
            return DbContext.ProductCategories.Find(id);
        }

        public IEnumerable<ProductCategory> GetList()
        {
            return DbContext.ProductCategories.Where(s => s.Status != ProductCategoryStatus.Deleted).ToList();
        }

        public bool Update(ProductCategory existItem, ProductCategory item, ModelStateDictionary state)
        {
            if(state.IsValid)
            {
                existItem.Name = item.Name;
                existItem.Description = item.Description;
                existItem.LevelOneProductCategoryCode = item.LevelOneProductCategoryCode;
                existItem.UpdatedAt = DateTime.Now;
                existItem.UpdatedBy = userService.GetCurrentUserName();
                DbContext.ProductCategories.AddOrUpdate(existItem);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public bool UpdateWithImage(ProductCategory existItem, ProductCategory item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            throw new NotImplementedException();
        }

        public void ValidateCategory(ProductCategory item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void Validate(ProductCategory item, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(item.Code))
            {
                state.AddModelError("Code", "Product Category Code is required.");
            }
            var list = DbContext.ProductCategories.Where(s => s.Code.Contains(item.Code)).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("Code", "Product Category Code already exist.");
            }
        }

        public void DisposeDb()
        {
            DbContext.Dispose();
        }

        public bool UpdateNumber(ProductCategory existItem, ProductCategory item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}