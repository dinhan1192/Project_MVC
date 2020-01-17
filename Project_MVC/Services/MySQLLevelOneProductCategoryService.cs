using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_MVC.Models.LevelOneProductCategory;

namespace Project_MVC.Services
{
    public class MySQLLevelOneProductCategoryService : ICRUDService<LevelOneProductCategory>
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        public MySQLLevelOneProductCategoryService()
        {
            DbContext = new MyDbContext();
        }
        public bool Create(LevelOneProductCategory item, ModelStateDictionary state)
        {
            ValidateCode(item, state);
            //var errors = state.Values.SelectMany(v => v.Errors);
            if (state.IsValid)
            {
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = null;
                item.DeletedAt = null;
                item.Status = LevelOneProductCategoryStatus.NotDeleted;
                DbContext.LevelOneProductCategories.Add(item);
                DbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool CreateWithImage(LevelOneProductCategory item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        {
            throw new NotImplementedException();
        }

        public bool Delete(LevelOneProductCategory item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                item.Status = LevelOneProductCategoryStatus.Deleted;
                item.DeletedAt = DateTime.Now;
                DbContext.LevelOneProductCategories.AddOrUpdate(item);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public LevelOneProductCategory Detail(string id)
        {
            return DbContext.LevelOneProductCategories.Find(id);
        }

        public bool Update(LevelOneProductCategory existItem, LevelOneProductCategory item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                existItem.Name = item.Name;
                existItem.Description = item.Description;
                existItem.UpdatedAt = DateTime.Now;
                DbContext.LevelOneProductCategories.AddOrUpdate(existItem);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public bool UpdateWithImage(LevelOneProductCategory existItem, LevelOneProductCategory item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            throw new NotImplementedException();
        }

        public void ValidateCategory(LevelOneProductCategory item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void ValidateCode(LevelOneProductCategory item, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(item.Code))
            {
                state.AddModelError("Code", "Level One Product Category Code is required.");
            }
            var list = DbContext.LevelOneProductCategories.Where(s => s.Code.Contains(item.Code)).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("Code", "Level OneProduct Category Code already exist.");
            }
        }

        public IEnumerable<LevelOneProductCategory> GetList()
        {
            return DbContext.LevelOneProductCategories.Where(s => s.Status != LevelOneProductCategoryStatus.Deleted);
        }

        public void DisposeDb()
        {
            DbContext.Dispose();
        }
    }
}