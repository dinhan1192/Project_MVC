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
using static Project_MVC.Models.LevelOneProductCategory;
using static Project_MVC.Models.ProductCategory;

namespace Project_MVC.Controllers
{
    [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
    public class ProductCategoriesController : Controller
    {
        //private MyDbContext _db;
        //public MyDbContext DbContext
        //{
        //    get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
        //    set { _db = value; }
        //}
        private ICRUDService<ProductCategory> mySQLProductCategoryService;
        private ICRUDService<LevelOneProductCategory> mySQLLevelOneProductCategoryService;

        public ProductCategoriesController()
        {
            mySQLProductCategoryService = new MySQLProductCategoryService();
            mySQLLevelOneProductCategoryService = new MySQLLevelOneProductCategoryService();
        }

        //public ActionResult OnMenuProductCategories(string code)
        //{
        //    ViewData["category"] = db.LevelOneProductCategories.Find(code);
        //    var listArticles = db.Articles.Where(s => s.CategoryId == id).ToList(); // lọc theo category
        //    return View("Index", listArticles);
        //}

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

            var productCategories = mySQLProductCategoryService.GetList();

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

            int pageSize = Constant.PageSize;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)productCategories.Count() / pageSize)
            };
            ViewBag.Page = thisPage;
            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            return View(productCategories.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }

        //public ActionResult IndexCustomer(string levelOneProductCategoryCode, int? page)
        //{
        //    // lúc đầu vừa vào thì sortOrder là null, cho nên gán NameSortParm = name_desc
        //    // Ấn vào link Full name thì lúc đó NameSortParm có giá trị là name_desc, sortOrder trên view được gán = NameSortParm cho nên sortOrder != null
        //    // và NameSortParm = ""
        //    // Ấn tiếp vào link Full Name thì sortOrder = "" cho nên NameSortParm = name_desc

        //    var productCategories = mySQLProductCategoryService.GetList();

        //    if (!String.IsNullOrEmpty(levelOneProductCategoryCode))
        //    {
        //        productCategories = productCategories.Where(s => s.LevelOneProductCategoryCode == levelOneProductCategoryCode);
        //    }

        //    int pageSize = Constant.PageSize;
        //    int pageNumber = (page ?? 1);
        //    ThisPage thisPage = new ThisPage()
        //    {
        //        CurrentPage = pageNumber,
        //        TotalPage = Math.Ceiling((double)productCategories.Count() / pageSize)
        //    };
        //    ViewBag.Page = thisPage;

        //    // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
        //    //return View(students.ToList().ToPagedList(pageNumber, pageSize));
        //    return View(productCategories.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        //}

        public ActionResult GetListLevelOneProductCategories()
        {
            //Console.WriteLine("123");
            //var list = db.ProductCategories.Where(s => s.Status != ProductCategoryStatus.Deleted).ToList();
            var list = mySQLLevelOneProductCategoryService.GetList();
            var newlist = list.Select(dep => new
            {
                dep.Code,
                dep.Name
            });
            return new JsonResult()
            {
                Data = newlist,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            };
        }

        // GET: ProductCategories/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = mySQLProductCategoryService.Detail(id);
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
        public ActionResult Create([Bind(Include = "Code,Name,Description,LevelOneProductCategoryCode")] ProductCategory productCategory)
        {
            //ModelStateDictionary state = ModelState;
            if (mySQLProductCategoryService.Create(productCategory, ModelState))
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
            ProductCategory productCategory = mySQLProductCategoryService.Detail(id);
            if (productCategory.LevelOneProductCategory == null)
            {
                productCategory.LevelOneProductCategoryNameAndCode = "";
            }
            else
            {
                productCategory.LevelOneProductCategoryNameAndCode = productCategory.LevelOneProductCategory.Code + " - " + productCategory.LevelOneProductCategory.Name;
            }
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
        public ActionResult Edit([Bind(Include = "Code,Name,Description,LevelOneProductCategoryCode")] ProductCategory productCategory)
        {
            //ModelStateDictionary state = ModelState;

            if (productCategory == null || productCategory.Code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existProductCategory = mySQLProductCategoryService.Detail(productCategory.Code);
            if (existProductCategory == null || existProductCategory.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLProductCategoryService.Update(existProductCategory, productCategory, ModelState))
            {
                return RedirectToAction("Index");
            }

            return View(productCategory);
        }

        // GET: ProductCategories/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProductCategory productCategory = DbContext.ProductCategories.Find(id);
        //    if (productCategory == null || productCategory.IsDeleted())
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(productCategory);
        //}

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //ModelStateDictionary state = ModelState;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existProductCategory = mySQLProductCategoryService.Detail(id);
            if (existProductCategory == null || existProductCategory.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLProductCategoryService.Delete(existProductCategory, ModelState))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mySQLProductCategoryService.DisposeDb();
            }
            base.Dispose(disposing);
        }
    }
}
