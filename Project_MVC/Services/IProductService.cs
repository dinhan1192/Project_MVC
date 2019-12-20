using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Services
{
    interface IProductService
    {
        bool Create(Product product, ModelStateDictionary state);
        bool Update(Product existProduct, Product product, ModelStateDictionary state);
        bool Delete(Product product, ModelStateDictionary state);
        bool Detail(int? id);
        void Validate(Product product, ModelStateDictionary state);
    }
}