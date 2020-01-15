using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Project_MVC.App_Start;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project_MVC.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }

        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }
        //private RoleManager<AppRole> roleManager;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //public AccountsController()
        //{
        //    //var roleStore = new RoleStore<AppRole>(dbContext);
        //    //roleManager = new RoleManager<AppRole>(roleStore);
        //    var userStore = new UserStore<AppUser>(dbContext);
        //    userManager = new UserManager<AppUser>(userStore);
        //}
        public AccountsController()
        {
        }

        //public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        //{
        //    UserManager = userManager;
        //    SignInManager = signInManager;
        //}

        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        //    }
        //    private set
        //    {
        //        _signInManager = value;
        //    }
        //}

        [Authorize(Roles = "Admin")]
        //[Authorize]
        public ActionResult AddUserToRole(string accountId, string roleName)
        {
            UserManager.AddToRole(accountId, roleName);
            return Redirect("/Products/Index");
        }

        // GET: Accounts
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(AppUser user)
        {
            if (ModelState.IsValid)
            {
                var account = new AppUser()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Gender = user.Gender,
                    BirthDay = DateTime.Now
                };

                ViewBag.UserName = user.UserName;

                IdentityResult result = await UserManager.CreateAsync(account, user.Password);
                Debug.WriteLine("@@@" + result.Succeeded);
                if (result.Succeeded)
                {
                    //Debug.WriteLine("@@@" + account.Id);
                    UserManager.AddToRole(account.Id, "User");
                    //var authenticationManager = System.Web.HttpContext.Current
                    //    .GetOwinContext().Authentication;
                    //var userIdentity = UserManager.CreateIdentity(
                    //    account, DefaultAuthenticationTypes.ApplicationCookie);
                    //authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(account.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Accounts", new { userId = account.Id, code = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(account.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    //string code = await UserManager.GenerateEmailConfirmationTokenAsync(account.Id);
                    //await UserManager.SendEmailAsync(account.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"http://google.com.vn\">here</a>");

                    return View("PageWaitWhileRedirected");
                }
                AddErrors(result);
            }
            return View(user);
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult ProcessLogin(string username, string password)
        {
            var user = UserManager.Find(username, password);
            if (user != null)
            {
                var authenticationManager = System.Web.HttpContext.Current
                    .GetOwinContext().Authentication;
                var userIdentity = UserManager.CreateIdentity(
                    user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
                return Redirect("/Products/Index");
            }
            return View("Login");
        }

        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                var authenticationManager = System.Web.HttpContext.Current
                    .GetOwinContext().Authentication;
                var account = DbContext.Users.Find(userId);
                var userIdentity = UserManager.CreateIdentity(
                    account, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                return View("ConfirmEmail");
            }
            return View("Error");
        }

        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(AppUser model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Accounts", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                return RedirectToAction("ForgotPasswordConfirmation", "Accounts");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            var user = DbContext.Users.Find(userId);
            user.CodeToResetPassword = code;
            return code == null ? View("Error") : View(user);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(AppUser model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Accounts");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.CodeToResetPassword, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Accounts");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        public ActionResult Logout()
        {
            var authenticationManager = System.Web.HttpContext.Current
                .GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return RedirectToAction("Login", "Accounts");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}