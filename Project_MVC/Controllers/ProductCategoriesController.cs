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
using static Project_MVC.Models.ProductCategory;

namespace Project_MVC.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private MyDbContext db = new MyDbContext();
        private IProductCategoryService mySQLProductCategoryService;

        public ProductCategoriesController()
        {
            mySQLProductCategoryService = new MySQLProductCategoryService();
        }

        // GET: ProductCategories
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            // lúc đầu vừa vào thì sortOrder là null, cho nên gán NameSortParm = name_desc
            // Ấn vào link Full name thì lúc đó NameSortParm có giá trị là name_desc, sortOrder trên view được gán = NameSortParm cho nên sortOrder != null
            // và NameSortParm = ""
            // Ấn tiếp vào link Full Name thì sortOrder = "" cho nên NameSortParm = name_desc
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var productCategories = db.ProductCategories.Where(s => s.Status != ProductCategoryStatus.Deleted);

            if (!String.IsNullOrEmpty(searchString))
            {
                productCategories = productCategories.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    productCategories = productCategories.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    productCategories = productCategories.OrderBy(s => s.CreatedAt);
                    break;
                case "date_desc":
                    productCategories = productCategories.OrderByDescending(s => s.CreatedAt);
                    break;
                default:
                    productCategories = productCategories.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            ViewBag.currentPage = pageNumber;
            ViewBag.totalPage = Math.Ceiling((double)productCategories.Count() / pageSize);
            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            return View(productCategories.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }

        // GET: ProductCategories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null || productCategory.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // GET: ProductCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,Description")] ProductCategory productCategory)
        {
            ModelStateDictionary state = ModelState;
            if (mySQLProductCategoryService.Create(productCategory, state))
            {
                return RedirectToAction("Index");
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null || productCategory.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Description")] ProductCategory productCategory)
        {
            ModelStateDictionary state = ModelState;

            if (productCategory == null || productCategory.Code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existProductCategory = db.ProductCategories.Find(productCategory.Code);
            if (existProductCategory == null || existProductCategory.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLProductCategoryService.Update(existProductCategory, productCategory, state))
            {
                return RedirectToAction("Index");
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null || productCategory.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ModelStateDictionary state = ModelState;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existProductCategory = db.ProductCategories.Find(id);
            if (existProductCategory == null || existProductCategory.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLProductCategoryService.Delete(existProductCategory, state))
            {
                return RedirectToAction("Index");
            }
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
