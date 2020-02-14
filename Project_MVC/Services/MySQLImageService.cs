﻿using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Services
{
    public class MySQLImageService : IImageService
    {
        private MyDbContext _db;
        private IUserService userService;

        public MySQLImageService()
        {
            userService = new UserService();
        }

        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        public bool Delete(LectureVideo existItem, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                DbContext.LectureVideos.Remove(existItem);
                DbContext.SaveChanges();

                return true;
            }

            return false;
        }

        public LectureVideo Detail(int? fileId)
        {
            return DbContext.LectureVideos.Find(fileId);
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
                            img.CreatedAt = DateTime.Now;
                            img.CreatedBy = userService.GetCurrentUserName();
                            imageList.Add(img);
                        }
                    }
                }
                return imageList;
            }

            return null;
        }

        public List<LectureVideo> SaveVideo2List(int? id, IEnumerable<HttpPostedFileBase> videos, ModelStateDictionary state)
        {
            if (videos != null)
            {
                var videoList = new List<LectureVideo>();
                foreach (var video in videos)
                {
                    if (video != null)
                    {
                        using (var br = new BinaryReader(video.InputStream))
                        {
                            ValidateVideo(video.FileName, state);
                            var data = br.ReadBytes(video.ContentLength);
                            var contentType = video.ContentType;
                            var vid = new LectureVideo { LectureId = id };
                            vid.Name = video.FileName;
                            if (char.IsDigit(vid.Name[1]))
                            {
                                vid.DisplayOrder = Convert.ToInt32(vid.Name.Substring(0, 2));
                            }
                            else
                            {
                                vid.DisplayOrder = Convert.ToInt32(vid.Name[0].ToString());
                            }
                            ValidateVideoDisplayOrder(vid.DisplayOrder, (int)id, state);
                            vid.VideoData = data;
                            vid.ContentType = contentType;
                            vid.CreatedAt = DateTime.Now;
                            vid.CreatedBy = userService.GetCurrentUserName();
                            videoList.Add(vid);
                        }
                    }
                }
                return videoList;
            }

            return null;
        }

        public void ValidateVideo(string videoName, ModelStateDictionary state)
        {
            if (!char.IsDigit(videoName[0]))
            {
                state.AddModelError("LectureVideoValidation", "Tên file của Video bài giảng phải bắt đầu bằng số.");
            }
        }

        public void ValidateVideoDisplayOrder(int displayOrder, int parentId, ModelStateDictionary state)
        {
            var list = DbContext.LectureVideos.Where(s => s.DisplayOrder == displayOrder && s.LectureId == parentId).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("LectureVideoValidation", "Có video trùng thứ tự sắp xếp.");
            }

            if(displayOrder < Constant.FirstDisplayOrder)
            {
                state.AddModelError("LectureVideoValidation", "Số thứ tự không thể nhỏ hơn 1");
            }
        }
    }
}