using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Project_MVC.Services
{
    interface IProductCategoryService
    {
        bool Create(ProductCategory productCategory, ModelStateDictionary state);
        bool Update(ProductCategory existProductCategory, ProductCategory productCategory, ModelStateDictionary state);
        bool Delete(ProductCategory productCategory, ModelStateDictionary state);
        ProductCategory Detail(ProductCategory productCategory);
    }
}
