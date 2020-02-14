using Microsoft.AspNet.Identity.Owin;
using Project_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Project_MVC.Models.Order;

namespace Project_MVC.Services
{
    public class MySQLOrderService : IOrderService
    {
        private MyDbContext _db;
        public MyDbContext DbContext
        {
            get { return _db ?? HttpContext.Current.GetOwinContext().Get<MyDbContext>(); }
            set { _db = value; }
        }

        private IUserService userService;

        public MySQLOrderService()
        {
            userService = new UserService();
        }
        public int? Create(Order item)
        {
            var existOrder = DbContext.Orders.Where(s => s.ShipName == item.ShipName).ToList();
            var code = item.OrderDetails.FirstOrDefault().ProductCode;
            var existOrderDetail = DbContext.OrderDetails.Where(s => s.ProductCode == code).ToList();
            if (existOrder != null && existOrder.Count > 0 && existOrderDetail != null && existOrderDetail.Count > 0)
                return existOrderDetail.FirstOrDefault().OrderId;

            item.CreatedAt = DateTime.Now;
            item.UpdatedAt = null;
            item.DeletedAt = null;
            item.CreatedBy = userService.GetCurrentUserName();
            item.Status = OrderStatus.NotDeleted;
            DbContext.Orders.Add(item);
            DbContext.SaveChanges();

            return item.Id;
        }

        public bool CreateWithImage(Order item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images, IEnumerable<HttpPostedFileBase> videos)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Order item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public Order Detail(int? id)
        {
            return DbContext.Orders.Find(id);
        }

        public void DisposeDb()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetList()
        {
            throw new NotImplementedException();
        }

        public int? UpdateStatus(Order item)
        {
            item.UpdatedAt = null;
            item.UpdatedBy = userService.GetCurrentUserName();
            item.Status = OrderStatus.Paid;
            DbContext.Orders.AddOrUpdate(item);
            DbContext.SaveChanges();

            return item.Id;
        }

        public bool UpdateWithImage(Order existItem, Order item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            throw new NotImplementedException();
        }

        public void Validate(Order item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public void ValidateCategory(Order item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool ValidateStringCode(string code)
        {
            throw new NotImplementedException();
        }

        public bool Update(Order existItem, Order item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }
    }
}