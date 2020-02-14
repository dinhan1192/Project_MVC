using Project_MVC.Models;
using Project_MVC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Controllers
{
    public class HomeController : Controller
    {
        private ICRUDService<Product> mySQLProductService;

        public HomeController()
        {
            mySQLProductService = new MySQLProductService();
        }

        public ActionResult Index()
        {
            return View(mySQLProductService.GetList().OrderBy(s => s.CreatedAt).Take(6));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}