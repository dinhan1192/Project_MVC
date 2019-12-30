using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Project_MVC.Services
{
    public class MySQLImageService : IImageService
    {
        private MyDbContext db = new MyDbContext();
        public List<ProductImage> SaveImage2List(string code, IEnumerable<HttpPostedFileBase> images)
        {
            if (images != null)
            {
                var imageList = new List<ProductImage>();
                foreach (var image in images)
                {
                    if (image != null)
                    {
                        using (var br = new BinaryReader(image.InputStream))
                        {
                            var data = br.ReadBytes(image.ContentLength);
                            var img = new ProductImage { ProductCode = code };
                            img.ImageData = data;
                            imageList.Add(img);
                        }
                    }
                }
                return imageList;
            }

            return null;
        }
    }
}