using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_MVC.Services
{
    public class UserService : IUserService
    {
        public string GetCurrentUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }
    }
}