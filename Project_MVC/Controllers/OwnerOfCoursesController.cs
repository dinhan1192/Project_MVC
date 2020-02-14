using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_MVC.Models;

namespace Project_MVC.Controllers
{
    public class OwnerOfCoursesController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: OwnerOfCourses
        public ActionResult Index()
        {
            return View(db.OwnerOfCourses.ToList());
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
        public ActionResult Create([Bind(Include = "Code,Name,Description,CreatedAt,UpdatedAt,DeletedAt,CreatedBy,UpdatedBy,DeletedBy,Status")] OwnerOfCourse ownerOfCourse)
        {
            if (ModelState.IsValid)
            {
                db.OwnerOfCourses.Add(ownerOfCourse);
                db.SaveChanges();
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
        public ActionResult Edit([Bind(Include = "Code,Name,Description,CreatedAt,UpdatedAt,DeletedAt,CreatedBy,UpdatedBy,DeletedBy,Status")] OwnerOfCourse ownerOfCourse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ownerOfCourse).State = EntityState.Modified;
                db.SaveChanges();
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
