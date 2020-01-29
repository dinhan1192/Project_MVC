using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Project_MVC.Models;
using Project_MVC.Utils;

namespace Project_MVC.Controllers
{
    public class LevelOneMenusController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: LevelOneMenus
        public ActionResult Index()
        {
            return View(db.LevelOneMenus.ToList());
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetActions(string name)
        {
            //Your Code For Getting Physicans Goes Here
            var actionList = MenuUtil.GetActionNames(name).Select(m => new SelectListItem()
    {
        Text = m.Name,
        Value = m.Name
    });

            return Json(actionList, JsonRequestBehavior.AllowGet);
        }

        // GET: LevelOneMenus/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelOneMenu levelOneMenu = db.LevelOneMenus.Find(id);
            if (levelOneMenu == null)
            {
                return HttpNotFound();
            }
            return View(levelOneMenu);
        }

        // GET: LevelOneMenus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LevelOneMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,ActionName,ControllerName,Description")] LevelOneMenu levelOneMenu)
        {
            if (ModelState.IsValid)
            {
                db.LevelOneMenus.Add(levelOneMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(levelOneMenu);
        }

        // GET: LevelOneMenus/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelOneMenu levelOneMenu = db.LevelOneMenus.Find(id);
            if (levelOneMenu == null)
            {
                return HttpNotFound();
            }
            return View(levelOneMenu);
        }

        // POST: LevelOneMenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,ActionName,ControllerName,Description")] LevelOneMenu levelOneMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(levelOneMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(levelOneMenu);
        }

        // GET: LevelOneMenus/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelOneMenu levelOneMenu = db.LevelOneMenus.Find(id);
            if (levelOneMenu == null)
            {
                return HttpNotFound();
            }
            return View(levelOneMenu);
        }

        // POST: LevelOneMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LevelOneMenu levelOneMenu = db.LevelOneMenus.Find(id);
            db.LevelOneMenus.Remove(levelOneMenu);
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
