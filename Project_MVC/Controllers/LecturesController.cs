using Project_MVC.Models;
using Project_MVC.Services;
using Project_MVC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static Project_MVC.Models.Lecture;

namespace Project_MVC.Controllers
{
    [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
    public class LecturesController : Controller
    {
        private ICRUDService<Lecture> mySQLLectureService;
        private ICRUDService<Product> mySQLProductService;
        private IImageService mySQLImageService;

        public LecturesController()
        {
            mySQLLectureService = new MySQLLectureService();
            mySQLImageService = new MySQLImageService();
            mySQLProductService = new MySQLProductService();
        }
        // GET: Lectures
        public ActionResult Index(string productCode)
        {
            ViewBag.ListTopCourse = mySQLProductService.GetList();
            ViewBag.CurrentCourse = mySQLProductService.Detail(productCode);
            return View(mySQLLectureService.GetList().Where(s => s.ProductCode == productCode && s.Status == LectureStatus.NotDeleted));
        }

        public ActionResult AddLecture(string id)
        {
            if(string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewBag.ProductCode = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLecture([Bind(Include = "Name,Description,ProductCode,DisplayOrder")] Lecture lecture, IEnumerable<HttpPostedFileBase> videos)
        {
            //ModelStateDictionary state = ModelState;

            if (mySQLLectureService.CreateWithImage(lecture, ModelState, null, videos))
            {
                return RedirectToAction("Details", "Products", new { id = lecture.ProductCode });
            }

            return View(lecture);
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecture lecture = mySQLLectureService.Detail(id);
            if (lecture == null || lecture.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            return View(lecture);
        }

        public ActionResult DetailVideos(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecture lecture = mySQLLectureService.Detail(id);
            if (lecture == null || lecture.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            ViewBag.CurrentCourse = mySQLProductService.Detail(lecture.ProductCode);
            ViewBag.ListLectureVideos = lecture.LectureVideos.ToList();

            int pageSize = Constant.PageVideoSize;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)lecture.LectureVideos.Count() / pageSize)
            };
            ViewBag.Page = thisPage;

            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            return View(lecture.LectureVideos.OrderBy(s => s.DisplayOrder).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList());
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lecture lecture = mySQLLectureService.Detail(id);
            ViewBag.ProductCode = lecture.ProductCode;
            if (lecture == null || lecture.IsDeleted())
            {
                return HttpNotFound();
            }
            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", product.ProductCategoryId);
            return View(lecture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ProductCode,DisplayOrder")] Lecture lecture, IEnumerable<HttpPostedFileBase> videos)
        {
            //ModelStateDictionary state = ModelState;
            if (lecture == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existLecture = mySQLLectureService.Detail(lecture.Id.ToString());
            if (existLecture == null || existLecture.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLLectureService.UpdateWithImage(existLecture, lecture, ModelState, videos))
            {
                return RedirectToAction("Details", "Products", new { id = existLecture.ProductCode });
            }

            return View(lecture);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //ModelStateDictionary state = ModelState;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existLecture = mySQLLectureService.Detail(id);
            var productCode = existLecture.ProductCode;
            if (existLecture == null || existLecture.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLLectureService.Delete(existLecture, ModelState))
            {
                return RedirectToAction("Details", "Products", new { id = productCode });
            }
            return RedirectToAction("Details", "Products", new { id = productCode });
        }

        [HttpPost, ActionName("DeleteVideo")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteVideo(string id)
        {
            //ModelStateDictionary state = ModelState;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existLectureVideo = mySQLImageService.Detail(Utility.GetNullableInt(id));
            var lectureId = existLectureVideo.LectureId;

            if (existLectureVideo == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLImageService.Delete(existLectureVideo, ModelState))
            {
                return RedirectToAction("Details", "Lectures", new { id = lectureId });
            }
            return RedirectToAction("Details", "Lectures", new { id = lectureId });
        }
    }
}