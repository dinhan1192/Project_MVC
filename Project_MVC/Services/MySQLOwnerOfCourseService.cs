using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_MVC.Models.OwnerOfCourse;

namespace Project_MVC.Services
{
    public class MySQLOwnerOfCourseService : ICRUDService<OwnerOfCourse>
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private IImageService mySQLImageService;

        private IUserService userService;

        public MySQLOwnerOfCourseService()
        {
            userService = new UserService();
            mySQLImageService = new MySQLImageService();
        }
        public bool Create(OwnerOfCourse item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool CreateWithImage(OwnerOfCourse item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        {
            Validate(item, state);
            ValidateCategory(item, state);
            if (state.IsValid)
            {
                //product.ProductCategoryId = Utils.Utility.GetNullableInt(product.ProductCategoryNameAndId.Split(' ')[0]);
                //product.ProductCategoryName = product.ProductCategoryNameAndId.Substring(product.ProductCategoryNameAndId.IndexOf('-') + 2);
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = DateTime.Now;
                item.UpdatedBy = userService.GetCurrentUserName();
                item.DeletedAt = null;
                item.CreatedBy = userService.GetCurrentUserName();
                item.Status = OwnerOfCourseStatus.NotDeleted;
                DbContext.OwnerOfCourses.Add(item);
                // add image to table ProductImages
                var lstImages = mySQLImageService.SaveImage2List(item.Code, Constant.OwnerOfCourseImage, images);
                foreach(var image in lstImages)
                {
                    item.ImageData = image.ImageData;
                    break;
                }
                //item.ProductVideos = mySQLImageService.SaveVideo2List(item.Code, videos);
                //
                DbContext.SaveChanges();
                return true;

            }

            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", product.ProductCategoryId);
            return false;
        }

        public bool Delete(OwnerOfCourse item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public OwnerOfCourse Detail(string id)
        {
            return DbContext.OwnerOfCourses.Find(id);
        }

        public void DisposeDb()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OwnerOfCourse> GetList()
        {
            return DbContext.OwnerOfCourses.Where(s => s.Status != OwnerOfCourseStatus.Deleted).ToList();
        }

        public bool Update(OwnerOfCourse existItem, OwnerOfCourse item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool UpdateWithImage(OwnerOfCourse existItem, OwnerOfCourse item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            ValidateCategory(item, state);
            if (state.IsValid)
            {
                existItem.Name = item.Name;
                existItem.ProductCategoryCode = item.ProductCategoryCode;
                //existItem.NumberOfLeture = item.NumberOfLeture;
                existItem.Occupation = item.Occupation;
                existItem.Description = item.Description;
                existItem.UpdatedAt = DateTime.Now;
                existItem.UpdatedBy = userService.GetCurrentUserName();
                var lstImages = mySQLImageService.SaveImage2List(item.Code, Constant.OwnerOfCourseImage, images);
                foreach (var image in lstImages)
                {
                    existItem.ImageData = image.ImageData;
                    break;
                }
                //var list = existItem.ProductImages;
                DbContext.OwnerOfCourses.AddOrUpdate(existItem);
                // add image to table ProductImages
                //
                DbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public void Validate(OwnerOfCourse item, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(item.Code))
            {
                state.AddModelError("Code", "Owner Of Course Code is required.");
            }
            var list = DbContext.OwnerOfCourses.Where(s => s.Code.Contains(item.Code)).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("Code", "Owner Of Course Code already exist.");
            }
        }

        public void ValidateCategory(OwnerOfCourse item, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(item.ProductCategoryCode))
            {
                state.AddModelError("ProductCategoryNameAndCode", "Product Category is required.");
            }
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}