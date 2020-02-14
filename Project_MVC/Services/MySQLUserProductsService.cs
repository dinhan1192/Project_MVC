using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Services
{
    public class MySQLUserProductsService : ICRUDService<UserProduct>
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private IUserService userService;

        public MySQLUserProductsService()
        {
            userService = new UserService();
        }

        public bool Create(UserProduct item, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                DbContext.UserProducts.Add(item);
                DbContext.SaveChanges();
                return true;
            }

            return false;
        }

        public bool CreateWithImage(UserProduct item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        {
            throw new NotImplementedException();
        }

        public bool Delete(UserProduct item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public UserProduct Detail(string id)
        {
            throw new NotImplementedException();
        }

        public void DisposeDb()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserProduct> GetList()
        {
            throw new NotImplementedException();
        }

        public bool Update(UserProduct existItem, UserProduct item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool UpdateWithImage(UserProduct existItem, UserProduct item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            throw new NotImplementedException();
        }

        public void Validate(UserProduct item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void ValidateCategory(UserProduct item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}