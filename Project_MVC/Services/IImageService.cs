using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Project_MVC.Services
{
    interface IImageService
    {
        List<ProductImage> SaveImage2List(string code, IEnumerable<HttpPostedFileBase> images);
        List<ProductVideo> SaveVideo2List(string code, IEnumerable<HttpPostedFileBase> videos);
        ProductVideo Detail(int? fileId);
    }
}
