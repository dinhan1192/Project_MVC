using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Project_MVC.Utils
{
    public class MenuUtil
    {
        private static MyDbContext _db;
        public static MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
        private static List<LevelOneProductCategory> _listLevelOneProductCategories;
        private static List<ProductCategory> _listProductCategories;

        //public MenuUtil()
        //{
        //    DbContext = new MyDbContext();
        //}

        public static List<LevelOneProductCategory> GetLevelOneProductCategories()
        {
            if (_listLevelOneProductCategories == null || _listLevelOneProductCategories.Count == 0)
            {
                _listLevelOneProductCategories = DbContext.LevelOneProductCategories.ToList();
            }
            return _listLevelOneProductCategories;
        }

        public static void SetLevelOneProductCategories(List<LevelOneProductCategory> lstLevelOneProductCategories)
        {
            _listLevelOneProductCategories = lstLevelOneProductCategories;
        }

        public static List<ProductCategory> GetProductCategories(string Code)
        {
            //if (_listProductCategories == null || _listProductCategories.Count == 0)
            //{
            //    _listProductCategories = DbContext.ProductCategories.Where(s => s.LevelOneProductCategory.Code == Code).ToList();
            //}
            _listProductCategories = DbContext.ProductCategories.Where(s => s.LevelOneProductCategory.Code == Code).ToList();
            return _listProductCategories;
        }

        public static void SetProductCategories(List<ProductCategory> lstProductCategories)
        {
            _listProductCategories = lstProductCategories;
        }

        //public static List<SelectListItem> GetCategoriesAsDropDownList()
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();
        //    if (_listCategories == null)
        //    {
        //        _listCategories = db.Categories.ToList();
        //    }

        //    foreach (var category in _listCategories)
        //    {
        //        list.Add(new SelectListItem { Text = category.Name, Value = category.Id.ToString() });
        //    }
        //    return list;
        //}
    }

    public static class RolesUtil
    {
        public static bool IsInAnyRole(this IPrincipal user, string[] roles)
        {
            //Check if authenticated first (optional)
            if (!user.Identity.IsAuthenticated) return false;
            //foreach(var role in roles)
            //{
            //    if (user.IsInRole(role))
            //    {
            //        return true;
            //    }
            //}
            //return false;
            return roles.Any(user.IsInRole);
        }
    }
}