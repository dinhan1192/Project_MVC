using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Project_MVC.Models;
using Project_MVC.Services;
using static Project_MVC.Models.Product;
using static Project_MVC.Models.ProductCategory;

namespace Project_MVC.Controllers
{
    public class ProductsController : Controller
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
        private ICRUDService<Product> mySQLProductService;

        public ProductsController()
        {
            mySQLProductService = new MySQLProductService();
        }

        [HttpGet]
        public FileResult Video(int? fileId)
        {
            var video = DbContext.ProductVideos.Find(fileId);
            if (video.ContentType == null)
            {
                return File(video.VideoData, "video/mp4");
            }
            return File(video.VideoData, video.ContentType);
        }

        public ActionResult IndexCustomer(string productCategoryCode, string sortOrder, string searchString, string currentFilter, int? page)
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

            var products = DbContext.Products.Where(s => s.Status != ProductStatus.Deleted);

            if (!String.IsNullOrEmpty(productCategoryCode))
            {
                products = products.Where(s => s.ProductCategoryCode == productCategoryCode);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString) || s.Code.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    products = products.OrderBy(s => s.CreatedAt);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(s => s.CreatedAt);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = Constant.PageSize;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)products.Count() / pageSize)
            };
            ViewBag.Page = thisPage;

            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            return View(products.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }

        // GET: Products
        public ActionResult Index(string productCategoryCode, string sortOrder, string searchString, string currentFilter, int? page)
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

            var products = DbContext.Products.Where(s =>  s.Status != ProductStatus.Deleted);

            if(!String.IsNullOrEmpty(productCategoryCode))
            {
                products = products.Where(s => s.ProductCategoryCode == productCategoryCode);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString) || s.Code.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    products = products.OrderBy(s => s.CreatedAt);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(s => s.CreatedAt);
                    break;
                default:
                    products = products.OrderBy(s => s.Name);
                    break;
            }

            int pageSize = Constant.PageSize;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)products.Count() / pageSize)
            };
            ViewBag.Page = thisPage;
            
            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            return View(products.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }

        // Cái này là dùng cho AutoComplete
        public ActionResult GetListProductCategories()
     {
            //Console.WriteLine("123");
            //var list = db.ProductCategories.Where(s => s.Status != ProductCategoryStatus.Deleted).ToList();
            var list = DbContext.ProductCategories.Where(s => s.Status != ProductCategoryStatus.Deleted).Select(dep => new
            {
                dep.Code,
                dep.Name
            });
            return new JsonResult()
            {
                Data = list,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
        };
        }

        // GET: Products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = DbContext.Products.Find(id);
            if (product == null || product.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Code,Name,Price,Description,ProductCategoryCode,ProductCategoryNameAndCode")] Product product, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        {
            //ModelStateDictionary state = ModelState;

            if (mySQLProductService.CreateWithImage(product, ModelState, images, videos))
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = DbContext.Products.Find(id);
            product.ProductCategoryNameAndCode = product.ProductCategory.Code + " - " + product.ProductCategory.Name;
            if (product == null || product.IsDeleted())
            {
                return HttpNotFound();
            }
            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", product.ProductCategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Price,Description,ProductCategoryCode")] Product product, IEnumerable<HttpPostedFileBase> images)
        {
            //ModelStateDictionary state = ModelState;
            if (product == null || product.Code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existProduct = DbContext.Products.Find(product.Code);
            if (existProduct == null || existProduct.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLProductService.UpdateWithImage(existProduct, product, ModelState, images))
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = DbContext.Products.Find(id);
            if (product == null || product.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //ModelStateDictionary state = ModelState;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existProduct = DbContext.Products.Find(id);
            if (existProduct == null || existProduct.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLProductService.Delete(existProduct, ModelState))
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
