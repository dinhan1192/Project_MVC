using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_MVC.Models;
using Project_MVC.Utils;

namespace Project_MVC.Controllers
{
    public class LevelTwoMenusController : Controller
    {
        private MyDbContext db = new MyDbContext();

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

        // GET: LevelTwoMenus
        public ActionResult Index()
        {
            var levelTwoMenus = db.LevelTwoMenus.Include(l => l.LevelOneMenu);
            return View(levelTwoMenus.ToList());
        }

        // GET: LevelTwoMenus/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelTwoMenu levelTwoMenu = db.LevelTwoMenus.Find(id);
            if (levelTwoMenu == null)
            {
                return HttpNotFound();
            }
            return View(levelTwoMenu);
        }

        // GET: LevelTwoMenus/Create
        public ActionResult Create()
        {
            ViewBag.LevelOneMenuCode = new SelectList(db.LevelOneMenus, "Code", "Name");
            return View();
        }

        // POST: LevelTwoMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,ActionName,ControllerName,Description,CreatedAt,UpdatedAt,DeletedAt,CreatedBy,UpdatedBy,DeletedBy,Status,LevelOneMenuCode")] LevelTwoMenu levelTwoMenu)
        {
            if (ModelState.IsValid)
            {
                db.LevelTwoMenus.Add(levelTwoMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LevelOneMenuCode = new SelectList(db.LevelOneMenus, "Code", "Name", levelTwoMenu.LevelOneMenuCode);
            return View(levelTwoMenu);
        }

        // GET: LevelTwoMenus/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelTwoMenu levelTwoMenu = db.LevelTwoMenus.Find(id);
            if (levelTwoMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.LevelOneMenuCode = new SelectList(db.LevelOneMenus, "Code", "Name", levelTwoMenu.LevelOneMenuCode);
            return View(levelTwoMenu);
        }

        // POST: LevelTwoMenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,ActionName,ControllerName,Description,CreatedAt,UpdatedAt,DeletedAt,CreatedBy,UpdatedBy,DeletedBy,Status,LevelOneMenuCode")] LevelTwoMenu levelTwoMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(levelTwoMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LevelOneMenuCode = new SelectList(db.LevelOneMenus, "Code", "Name", levelTwoMenu.LevelOneMenuCode);
            return View(levelTwoMenu);
        }

        // GET: LevelTwoMenus/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelTwoMenu levelTwoMenu = db.LevelTwoMenus.Find(id);
            if (levelTwoMenu == null)
            {
                return HttpNotFound();
            }
            return View(levelTwoMenu);
        }

        // POST: LevelTwoMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LevelTwoMenu levelTwoMenu = db.LevelTwoMenus.Find(id);
            db.LevelTwoMenus.Remove(levelTwoMenu);
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
