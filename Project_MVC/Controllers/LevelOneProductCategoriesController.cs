using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using Project_MVC.Services;

namespace Project_MVC.Controllers
{
    [Authorize(Roles=Constant.Admin + "," + Constant.Employee)]
    public class LevelOneProductCategoriesController : Controller
    {
        //private MyDbContext _db;
        //public MyDbContext DbContext
        //{
        //    get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
        //    set { _db = value; }
        //}

        private ICRUDService<LevelOneProductCategory> mySQLLevelOneProductCategoryService;

        public LevelOneProductCategoriesController()
        {
            mySQLLevelOneProductCategoryService = new MySQLLevelOneProductCategoryService();
        }

        // GET: LevalOneProductCategories
        public ActionResult Index()
        {
            return View(mySQLLevelOneProductCategoryService.GetList());
        }

        // GET: LevalOneProductCategories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelOneProductCategory levelOneProductCategory = mySQLLevelOneProductCategoryService.Detail(id);
            if (levelOneProductCategory == null || levelOneProductCategory.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(levelOneProductCategory);
        }

        // GET: LevalOneProductCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LevalOneProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,Description,CreatedAt,UpdatedAt,DeletedAt,Status")] LevelOneProductCategory levelOneProductCategory)
        {
            if (mySQLLevelOneProductCategoryService.Create(levelOneProductCategory, ModelState))
            {
                return RedirectToAction("Index");
            }

            return View(levelOneProductCategory);
        }

        // GET: LevalOneProductCategories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LevelOneProductCategory levelOneProductCategory = mySQLLevelOneProductCategoryService.Detail(id);
            if (levelOneProductCategory == null || levelOneProductCategory.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(levelOneProductCategory);
        }

        // POST: LevalOneProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Description,CreatedAt,UpdatedAt,DeletedAt,Status")] LevelOneProductCategory levelOneProductCategory)
        {
            if (levelOneProductCategory == null || levelOneProductCategory.Code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existLevelOneProductCategory = mySQLLevelOneProductCategoryService.Detail(levelOneProductCategory.Code);
            if (existLevelOneProductCategory == null || existLevelOneProductCategory.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLLevelOneProductCategoryService.Update(existLevelOneProductCategory, levelOneProductCategory, ModelState))
            {
                return RedirectToAction("Index");
            }

            return View(levelOneProductCategory);
        }

        // GET: LevalOneProductCategories/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    LevelOneProductCategory levalOneProductCategory = DbContext.LevelOneProductCategories.Find(id);
        //    if (levalOneProductCategory == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(levalOneProductCategory);
        //}

        // POST: LevalOneProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //ModelStateDictionary state = ModelState;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existLevelOneProductCategory = mySQLLevelOneProductCategoryService.Detail(id);
            if (existLevelOneProductCategory == null || existLevelOneProductCategory.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLLevelOneProductCategoryService.Delete(existLevelOneProductCategory, ModelState))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mySQLLevelOneProductCategoryService.DisposeDb();
            }
            base.Dispose(disposing);
        }
    }
}
