using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_MVC.Models;
using Project_MVC.Services;

namespace Project_MVC.Controllers
{
    public class OwnerOfCoursesController : Controller
    {
        private MyDbContext db = new MyDbContext();
        private ICRUDService<OwnerOfCourse> mySQLOwnerOfCourseService;

        public OwnerOfCoursesController()
        {
            mySQLOwnerOfCourseService = new MySQLOwnerOfCourseService();
        }

        // GET: OwnerOfCourses
        public ActionResult Index()
        {
            var list = db.OwnerOfCourses.ToList();
            return View(list);
        }

        // GET: OwnerOfCourses/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerOfCourse ownerOfCourse = db.OwnerOfCourses.Find(id);
            if (ownerOfCourse == null)
            {
                return HttpNotFound();
            }
            return View(ownerOfCourse);
        }

        // GET: OwnerOfCourses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OwnerOfCourses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,Description,ProductCategoryCode,Occupation")] OwnerOfCourse ownerOfCourse, IEnumerable<HttpPostedFileBase> images)
        {
            if (mySQLOwnerOfCourseService.CreateWithImage(ownerOfCourse, ModelState, images, null))
            {
                return RedirectToAction("Index");
            }

            return View(ownerOfCourse);
        }

        // GET: OwnerOfCourses/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerOfCourse ownerOfCourse = db.OwnerOfCourses.Find(id);
            if (ownerOfCourse.ProductCategory == null)
            {
                ownerOfCourse.ProductCategoryNameAndCode = "";
            }
            else
            {
                ownerOfCourse.ProductCategoryNameAndCode = ownerOfCourse.ProductCategory.Code + " - " + ownerOfCourse.ProductCategory.Name;
            }
            if (ownerOfCourse == null)
            {
                return HttpNotFound();
            }
            return View(ownerOfCourse);
        }

        // POST: OwnerOfCourses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Description,ProductCategoryCode,Occupation")] OwnerOfCourse ownerOfCourse, IEnumerable<HttpPostedFileBase> images)
        {
            if (ownerOfCourse == null || ownerOfCourse.Code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existOwnerOfCourse = mySQLOwnerOfCourseService.Detail(ownerOfCourse.Code);
            if (existOwnerOfCourse == null || existOwnerOfCourse.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLOwnerOfCourseService.UpdateWithImage(existOwnerOfCourse, ownerOfCourse, ModelState, images))
            {
                return RedirectToAction("Index");
            }

            return View(ownerOfCourse);
        }

        // GET: OwnerOfCourses/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerOfCourse ownerOfCourse = db.OwnerOfCourses.Find(id);
            if (ownerOfCourse == null)
            {
                return HttpNotFound();
            }
            return View(ownerOfCourse);
        }

        // POST: OwnerOfCourses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OwnerOfCourse ownerOfCourse = db.OwnerOfCourses.Find(id);
            db.OwnerOfCourses.Remove(ownerOfCourse);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
