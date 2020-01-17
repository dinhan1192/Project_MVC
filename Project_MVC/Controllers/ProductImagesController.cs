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

namespace Project_MVC.Controllers
{
    [Authorize(Roles=Constant.Admin + "," + Constant.Employee)]
    public class ProductImagesController : Controller
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        // GET: ProductImages
        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var productImages = DbContext.ProductImages.Where(s => !string.IsNullOrEmpty(s.ProductCode));

            if (!String.IsNullOrEmpty(searchString))
            {
                productImages = productImages.Where(s => s.Product.Name.Contains(searchString));
            }

            int pageSize = Constant.PageSize;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)productImages.Count() / pageSize)
            };
            ViewBag.Page = thisPage;
            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            return View(productImages.OrderBy(s => s.ProductCode).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }

        // GET: ProductImages/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProductImage productImage = db.ProductImages.Find(id);
        //    if (productImage == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(productImage);
        //}

        // GET: ProductImages/Create
        //public ActionResult Create()
        //{
        //    ViewBag.ProductCode = new SelectList(db.Products, "Code", "Name");
        //    return View();
        //}

        // POST: ProductImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,ProductCode,SignImage")] ProductImage productImage)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ProductImages.Add(productImage);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ProductCode = new SelectList(db.Products, "Code", "Name", productImage.ProductCode);
        //    return View(productImage);
        //}

        // GET: ProductImages/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProductImage productImage = db.ProductImages.Find(id);
        //    if (productImage == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ProductCode = new SelectList(db.Products, "Code", "Name", productImage.ProductCode);
        //    return View(productImage);
        //}

        // POST: ProductImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,ProductCode,SignImage")] ProductImage productImage)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(productImage).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ProductCode = new SelectList(db.Products, "Code", "Name", productImage.ProductCode);
        //    return View(productImage);
        //}

        // GET: ProductImages/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ProductImage productImage = db.ProductImages.Find(id);
        //    if (productImage == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(productImage);
        //}

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductImage productImage = DbContext.ProductImages.Find(id);
            if(productImage == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            DbContext.ProductImages.Remove(productImage);
            DbContext.SaveChanges();
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
