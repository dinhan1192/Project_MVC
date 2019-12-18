using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_MVC.Services
{
    interface IProductCategoryService
    {
        ProductCategory Create(ProductCategory productCategory);
        ProductCategory Update(ProductCategory existProductCategory, ProductCategory productCategory);
        ProductCategory Delete(ProductCategory productCategory);
        ProductCategory Detail(ProductCategory productCategory);
    }
}
