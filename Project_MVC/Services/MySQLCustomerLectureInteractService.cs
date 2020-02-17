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

        public void UpdateRating(decimal rating, int? customerLectureInteractId)
        {
            var existCustomerLectureInteract = DbContext.CustomerLectureInteracts.Find(customerLectureInteractId);
            existCustomerLectureInteract.Rating = rating;
            DbContext.CustomerLectureInteracts.AddOrUpdate(existCustomerLectureInteract);
            DbContext.SaveChanges();
        }
    }
}