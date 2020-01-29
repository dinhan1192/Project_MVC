using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using Project_MVC.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_MVC.Models.Lecture;

namespace Project_MVC.Services
{
    public class MySQLLectureService : ICRUDService<Lecture>
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private IUserService userService;

        private IImageService mySQLImageService;

        public MySQLLectureService()
        {
            mySQLImageService = new MySQLImageService();
            userService = new UserService();
        }
        public bool Create(Lecture item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool CreateWithImage(Lecture item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        {
            //var error = state.Values.SelectMany(s => s.Errors);
            item.LectureVideos = mySQLImageService.SaveVideo2List(item.Id, videos, state);
            if (state.IsValid)
            {
                //product.ProductCategoryId = Utils.Utility.GetNullableInt(product.ProductCategoryNameAndId.Split(' ')[0]);
                //product.ProductCategoryName = product.ProductCategoryNameAndId.Substring(product.ProductCategoryNameAndId.IndexOf('-') + 2);
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = null;
                item.DeletedAt = null;
                item.CreatedBy = userService.GetCurrentUserName();
                item.Status = LectureStatus.NotDeleted;
                DbContext.Lectures.Add(item);
                // add video to table LectureVideos
                DbContext.SaveChanges();
                return true;

            }
            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", product.ProductCategoryId);
            return false;

            //try
            //{
            //    item.CreatedAt = DateTime.Now;
            //    item.UpdatedAt = null;
            //    item.DeletedAt = null;
            //    item.CreatedBy = userService.GetCurrentUserName();
            //    item.Status = LectureStatus.NotDeleted;
            //    DbContext.Lectures.Add(item);
            //    // add video to table LectureVideos
            //    item.LectureVideos = mySQLImageService.SaveVideo2List(item.Id, videos, state);

            //    DbContext.SaveChanges();
            //    return true;
            //}
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
        }

        public bool Delete(Lecture item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public Lecture Detail(string id)
        {
            return DbContext.Lectures.Find(Utility.GetNullableInt(id));
        }

        public void DisposeDb()
        {
            DbContext.Dispose();
        }

        public IEnumerable<Lecture> GetList()
        {
            throw new NotImplementedException();
        }

        public bool Update(Lecture existItem, Lecture item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool UpdateWithImage(Lecture existItem, Lecture item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> videos)
        {
            if (state.IsValid)
            {
                existItem.Name = item.Name;
                existItem.Description = item.Description;
                existItem.UpdatedAt = DateTime.Now;
                existItem.UpdatedBy = userService.GetCurrentUserName();
                //var list = existItem.ProductImages;
                DbContext.Lectures.AddOrUpdate(existItem);
                // add image to table ProductImages
                var videoList = mySQLImageService.SaveVideo2List(item.Id, videos, state);
                DbContext.LectureVideos.AddRange(videoList);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public void ValidateCategory(Lecture item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void ValidateCode(Lecture item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}