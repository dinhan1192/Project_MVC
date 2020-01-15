using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_MVC.Utils
{
    public class MenuUtil
    {
        private static MyDbContext db = new MyDbContext();
        private static List<LevelOneProductCategory> _listLevelOneProductCategories;
        private static List<ProductCategory> _listProductCategories;

        public static List<LevelOneProductCategory> GetLevelOneProductCategories()
        {
            if (_listLevelOneProductCategories == null || _listLevelOneProductCategories.Count == 0)
            {
                _listLevelOneProductCategories = db.LevelOneProductCategories.ToList();
            }
            return _listLevelOneProductCategories;
        }

        public static void SetLevelOneProductCategories(List<LevelOneProductCategory> lstLevelOneProductCategories)
        {
            _listLevelOneProductCategories = lstLevelOneProductCategories;
        }

        public static List<ProductCategory> GetProductCategories(string Code)
        {
            if (_listProductCategories == null || _listProductCategories.Count == 0)
            {
                _listProductCategories = db.ProductCategories.Where(s => s.LevelOneProductCategory.Code == Code).ToList();
            }
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
}