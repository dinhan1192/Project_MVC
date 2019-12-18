using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_MVC.Services
{
    interface IProductService
    {
        Product Create(Product product);
        Product Update(Product existProduct, Product product);
        Product Delete(Product product);
        Product Detail(Product product);
    }
}