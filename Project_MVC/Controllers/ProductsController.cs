using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using Project_MVC.Models;
using Project_MVC.Services;
using Project_MVC.Utils;
using static Project_MVC.Models.Order;
using static Project_MVC.Models.Product;
using static Project_MVC.Models.ProductCategory;

namespace Project_MVC.Controllers
{
    public class ProductsController : Controller
    {
        //private MyDbContext _db;
        //public MyDbContext DbContext
        //{
        //    get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
        //    set { _db = value; }
        //}
        private ICRUDService<Product> mySQLProductService;
        private ICRUDService<ProductCategory> mySQLProductCategoryService;
        private ICRUDService<OwnerOfCourse> mySQLOwnerOfCourseService;
        private IOrderService mySQLOrderService;
        private ICRUDService<UserProduct> mySQLUserProductService;
        private IUserService userService;
        private IImageService mySQLImageService;
        private IValidateService mySQLValidateService;

        public ProductsController()
        {
            mySQLProductService = new MySQLProductService();
            mySQLProductCategoryService = new MySQLProductCategoryService();
            mySQLImageService = new MySQLImageService();
            mySQLOwnerOfCourseService = new MySQLOwnerOfCourseService();
            mySQLOrderService = new MySQLOrderService();
            mySQLUserProductService = new MySQLUserProductsService();
            userService = new UserService();
            mySQLValidateService = new MySQLValidateService();
        }

        [HttpGet]
        public FileResult Video(int? fileId)
        {
            var video = mySQLImageService.Detail(fileId);
            if (video.ContentType == null)
            {
                return File(video.VideoData, "video/mp4");
            }
            return File(video.VideoData, video.ContentType);
        }

        public ActionResult CourseIntroduction(string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.ListTopCourse = mySQLProductService.GetList();
            ViewBag.ReturnUrlCourseIntro = Request.RawUrl;
            var productCategoryCode = mySQLProductService.Detail(productCode).ProductCategoryCode;
            var listTeachers = mySQLProductCategoryService.Detail(productCategoryCode).OwnerOfCourses.ToList();
            if(listTeachers != null && listTeachers.Count != 0)
            {
                ViewBag.Teachers = listTeachers;
            }
            
            return View(mySQLProductService.Detail(productCode));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterForCourse(string productCode, string userName, string returnUrlCourseIntro)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("Login", "Accounts", new { returnUrl = returnUrlCourseIntro });
            }

            var product = mySQLProductService.Detail(productCode);

            var order = new Order
            {
                TotalPrice = product.Price,
                MemberId = 1,
                // can sua Language
                PaymentTypeId = PaymentType.InternetBanking,
                ShipName = userName,
                Status = OrderStatus.Pending,
                //ShipPhone = cartInfo.ShipPhone,
                //ShipAddress = cartInfo.ShipAddress,
                OrderDetails = new List<OrderDetail>()
            };

            order.OrderDetails.Add(new OrderDetail()
            {
                ProductCode = product.Code,
                OrderId = order.Id,
                Quantity = 1,
                UnitPrice = product.Price,
            });

            var orderId = mySQLOrderService.Create(order);

            if (order.PaymentTypeId == Order.PaymentType.InternetBanking)
            {
                //Get Config Info
                // return sang trang ket qua dang ky
                string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
                string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
                                                                              // sau khi dang ky Merchant moi co
                string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma website
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
                {
                    //lblMessage.Text = "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config";
                    //return;
                }
                //Get payment input
                //OrderInfo order = new OrderInfo();
                ////Save order to db
                //order.OrderId = DateTime.Now.Ticks;
                //order.Amount = Convert.ToInt64(txtAmount.Text);
                //order.OrderDesc = txtOrderDesc.Text;
                //order.CreatedDate = DateTime.Now;
                string locale = "";
                //Build URL for VNPAY
                VnPayLibrary vnpay = new VnPayLibrary();
                var date = DateTime.Now;

                vnpay.AddRequestData("vnp_Version", "2.0.0");
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnpay.AddRequestData("vnp_Amount", (order.TotalPrice * 100).ToString());
                //if (cboBankCode.SelectedItem != null && !string.IsNullOrEmpty(cboBankCode.SelectedItem.Value))
                //{
                //    vnpay.AddRequestData("vnp_BankCode", cboBankCode.SelectedItem.Value);
                //}
                vnpay.AddRequestData("vnp_CreateDate", date.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_IpAddr", Utils.Utils.GetIpAddress());


                if (!string.IsNullOrEmpty(locale))
                {
                    // locale
                    vnpay.AddRequestData("vnp_Locale", "vn");
                }
                else
                {
                    vnpay.AddRequestData("vnp_Locale", "vn");
                }
                vnpay.AddRequestData("vnp_OrderInfo", "Đã tạo Order");
                vnpay.AddRequestData("vnp_OrderType", order.PaymentTypeId.ToString()); //default value: other
                vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                vnpay.AddRequestData("vnp_TxnRef", orderId.ToString());

                string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                //log.InfoFormat("VNPAY URL: {0}", paymentUrl);
                //Debug.WriteLine("VNPAY URL: {0}", paymentUrl);
                return Redirect(paymentUrl);
            }
            else if (order.PaymentTypeId == PaymentType.Cod)
            {
                return View();
            }
            else
            {
                return View();
            }
        }

        public ActionResult PaidResult()
        {
            var product = new Product();

            //log.InfoFormat("Begin VNPAY Return, URL={0}", Request.RawUrl);
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();
                //if (vnpayData.Count > 0)
                //{
                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                // }

                //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                var order = mySQLOrderService.Detail(Convert.ToInt32(orderId));
                product = mySQLProductService.Detail(order.OrderDetails.FirstOrDefault().ProductCode);

                var userProduct = new UserProduct()
                {
                    ProductCode = product.Code,
                    UserId = User.Identity.GetUserId()
                };

                //vnp_TransactionNo: Ma GD tai he thong VNPAY
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                //vnp_SecureHash: MD5 cua du lieu tra ve
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toan thanh cong
                        ViewBag.DisplayMsg = "Thanh toán thành công";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                        mySQLOrderService.UpdateStatus(order);
                        mySQLUserProductService.Create(userProduct, ModelState);
                    }
                    else
                    {
                        //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                        ViewBag.DisplayMsg = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;
                        //log.InfoFormat("Thanh toan loi, OrderId={0}, VNPAY TranId={1},ResponseCode={2}", orderId, vnpayTranId, vnp_ResponseCode);
                    }
                }
                else
                {
                    //log.InfoFormat("Invalid signature, InputData={0}", Request.RawUrl);
                    ViewBag.DisplayMsg = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }
            return View(product);
        }

        public ActionResult IndexCustomer(string productCategoryCode, string searchString, string currentFilter, int? page)
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

            var products = mySQLProductService.GetList();

            if (!String.IsNullOrEmpty(productCategoryCode))
            {
                products = products.Where(s => s.ProductCategoryCode == productCategoryCode);
                var productCategory = mySQLProductCategoryService.Detail(productCategoryCode);
                var list = productCategory.OwnerOfCourses.ToList();
                ViewBag.Teachers = list;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Name.Contains(searchString) || s.Code.Contains(searchString));
            }

            int pageSize = Constant.PageSizeOnCustomerPage;
            int pageNumber = (page ?? 1);
            ThisPage thisPage = new ThisPage()
            {
                CurrentPage = pageNumber,
                TotalPage = Math.Ceiling((double)products.Count() / pageSize),
                ProductCategoryCode = productCategoryCode,
                CurrentType = Constant.Customer
            };
            ViewBag.Page = thisPage;

            // nếu page == null thì lấy giá trị là 1, nếu không thì giá trị là page
            //return View(students.ToList().ToPagedList(pageNumber, pageSize));
            return View(products.Skip(pageSize * (pageNumber - 1)).Take(pageSize).OrderByDescending(s => s.UpdatedAt).ToList());
        }

        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
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

            var products = mySQLProductService.GetList();

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
                    products = products.OrderBy(s => s.UpdatedAt);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(s => s.UpdatedAt);
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

        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        // Cái này là dùng cho AutoComplete
        public ActionResult GetListProductCategories()
        {
            //Console.WriteLine("123");
            //var list = db.ProductCategories.Where(s => s.Status != ProductCategoryStatus.Deleted).ToList();
            var list = mySQLProductCategoryService.GetList();
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

        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        // Cái này là dùng cho AutoComplete
        public ActionResult GetListOwnerOfCourses()
        {
            //Console.WriteLine("123");
            //var list = db.ProductCategories.Where(s => s.Status != ProductCategoryStatus.Deleted).ToList();
            var list = mySQLOwnerOfCourseService.GetList();
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

        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        [HttpPost]
        public JsonResult ValidateCode(string code)
        {
            bool isValid = mySQLProductService.ValidateStringCode(code);
            return Json(isValid);
        }

        [HttpPost]
        public JsonResult ValidateUserProductsExist(string code, string id)
        {
            bool isValid = mySQLValidateService.ValidateUserProducts(code, id);
            return Json(isValid);
        }

        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        // GET: Products/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = mySQLProductService.Detail(id);
            if (product == null || product.IsDeleted())
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        // GET: Products/Create
        public ActionResult Create()
        {
            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name");
            return View();
        }

        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        // GET: Products/Create
        public PartialViewResult CreatePopup()
        {
            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name");
            return PartialView("_CreateProduct");
        }

        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePopup([Bind(Include = "Code,Name")] Product product)
        {
            //ModelStateDictionary state = ModelState;

            if (mySQLProductService.Create(product, ModelState))
            {
                return RedirectToAction("Edit", new { id = product.Code });
            }

            return PartialView("_CreateProduct", product);
            //return RedirectToAction("Index");
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
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
        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = mySQLProductService.Detail(id);
            if (product.ProductCategory == null)
            {
                product.ProductCategoryNameAndCode = "";
            }
            else
            {
                product.ProductCategoryNameAndCode = product.ProductCategory.Code + " - " + product.ProductCategory.Name;
            }
            if (product.OwnerOfCourse == null)
            {
                product.OwnerOfCourseNameAndCode = "";
            }
            else
            {
                product.OwnerOfCourseNameAndCode = product.OwnerOfCourse.Code + " - " + product.OwnerOfCourse.Name;
            }
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
        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Code,Name,Price,Description,ProductCategoryCode,OwnerOfCourseCode")] Product product, IEnumerable<HttpPostedFileBase> images, string ActionName)
        {
            //ModelStateDictionary state = ModelState;
            if (product == null || product.Code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existProduct = mySQLProductService.Detail(product.Code);
            if (existProduct == null || existProduct.IsDeleted())
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
            if (mySQLProductService.UpdateWithImage(existProduct, product, ModelState, images))
            {
                switch (ActionName)
                {
                    case "Save":
                        return RedirectToAction("Index");
                    case "AddLecture":
                        return RedirectToAction("AddLecture", "Lectures", new { id = product.Code });
                }
            }

            product.Lectures = existProduct.Lectures;

            return View(product);
        }

        //[Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        //[HttpPost]
        //public ActionResult EditNumberOfLecture(string id, string numberOfLectures)
        //{
        //    //ModelStateDictionary state = ModelState;
        //    var number = Convert.ToInt32(numberOfLectures);
        //    var existProduct = mySQLProductService.Detail(id);
        //    if (existProduct == null || existProduct.IsDeleted())
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
        //    }
        //    var product = new Product()
        //    {
        //        NumberOfLeture = number
        //    };
        //    if (mySQLProductService.UpdateNumber(existProduct, product, ModelState))
        //    {
        //        existProduct.NumberOfLeture = product.NumberOfLeture;
        //    }

        //    return View(existProduct);
        //}

        // GET: Products/Delete/5
        //[Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = DbContext.Products.Find(id);
        //    if (product == null || product.IsDeleted())
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(product);
        //}

        // POST: Products/Delete/5
        [Authorize(Roles = Constant.Admin + "," + Constant.Employee)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //ModelStateDictionary state = ModelState;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var existProduct = mySQLProductService.Detail(id);
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
                mySQLProductService.DisposeDb();
            }
            base.Dispose(disposing);
        }
    }
}
