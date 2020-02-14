using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
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

        private IUserService userService;

        public MySQLOwnerOfCourseService()
        {
            userService = new UserService();
        }
        public bool Create(OwnerOfCourse item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool CreateWithImage(OwnerOfCourse item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        {
            throw new NotImplementedException();
        }

        public bool Delete(OwnerOfCourse item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public OwnerOfCourse Detail(string id)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Validate(OwnerOfCourse item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void ValidateCategory(OwnerOfCourse item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}