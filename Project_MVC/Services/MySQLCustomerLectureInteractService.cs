using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Project_MVC.Services
{
    public class MySQLCustomerLectureInteractService : ICustomerLectureInteractService
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
       
        public void CreateRating(decimal rating, int? lectureId, string userId)
        {
            var item = new CustomerLectureInteract()
            {
                LectureId = lectureId,
                UserId = userId,
                Rating = rating
            };

            DbContext.CustomerLectureInteracts.Add(item);
            DbContext.SaveChanges();
        }

        public CustomerLectureInteract DetailByLectureIdAndUserId(int? lectureId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return DbContext.CustomerLectureInteracts.Where(s => s.LectureId == lectureId && s.UserId == userId).FirstOrDefault();
        }

        public List<CustomerLectureInteract> GetListByLectureId(int? lectureId)
        {
            return DbContext.CustomerLectureInteracts.Where(s => s.LectureId == lectureId).ToList();
        }

        public void UpdateProductRating(string productcode)
        {
            var existProduct = DbContext.Products.Find(productcode);
            var listLecture = DbContext.Lectures.Where(s => s.ProductCode == existProduct.Code && s.Rating != null).ToList();
            var countLecture = listLecture.Count;
            if (countLecture != 0 && listLecture != null)
            {
                if (existProduct.Rating == null)
                {
                    existProduct.Rating = 0;
                }

                existProduct.Rating = listLecture.Select(s => s.Rating).Sum() / countLecture;
            }

            DbContext.Products.AddOrUpdate(existProduct);
            DbContext.SaveChanges();
        }

        public string UpdateLectureRating(decimal rating, int? lectureId, string type)
        {
            var existLecture = DbContext.Lectures.Find(lectureId);
            var listCusLecInteract = DbContext.CustomerLectureInteracts.Where(s => s.LectureId == lectureId).ToList();
            var countCusLecInteract = listCusLecInteract.Count;
            if (countCusLecInteract != 0 && listCusLecInteract != null)
            {
                if (existLecture.Rating == null)
                {
                    existLecture.Rating = 0;
                }

                switch (type)
                {
                    case Constant.CreateRating:
                        existLecture.Rating = (existLecture.Rating + rating) / countCusLecInteract;
                        break;
                    case Constant.UpdateRating:
                        existLecture.Rating = listCusLecInteract.Select(s => s.Rating).Sum() / countCusLecInteract;
                        break;
                    default:
                        break;
                }
            }

            DbContext.Lectures.AddOrUpdate(existLecture);
            DbContext.SaveChanges();

            return DbContext.Lectures.Find(lectureId).ProductCode;
        }

        public void UpdateRating(decimal rating, int? customerLectureInteractId)
        {
            var existCustomerLectureInteract = DbContext.CustomerLectureInteracts.Find(customerLectureInteractId);
            existCustomerLectureInteract.Rating = rating;
            DbContext.CustomerLectureInteracts.AddOrUpdate(existCustomerLectureInteract);
            DbContext.SaveChanges();
        }
    }
}