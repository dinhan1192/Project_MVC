using Microsoft.AspNet.Identity.Owin;
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
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        public ProductVideo Detail(int? fileId)
        {
            return DbContext.ProductVideos.Find(fileId);
        }

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

        public List<ProductVideo> SaveVideo2List(string code, IEnumerable<HttpPostedFileBase> videos)
        {
            if (videos != null)
            {
                var videoList = new List<ProductVideo>();
                foreach (var video in videos)
                {
                    if (video != null)
                    {
                        using (var br = new BinaryReader(video.InputStream))
                        {
                            var data = br.ReadBytes(video.ContentLength);
                            var contentType = video.ContentType;
                            var vid = new ProductVideo { ProductCode = code };
                            vid.VideoData = data;
                            vid.ContentType = contentType;
                            videoList.Add(vid);
                        }
                    }
                }
                return videoList;
            }

            return null;
        }
    }
}