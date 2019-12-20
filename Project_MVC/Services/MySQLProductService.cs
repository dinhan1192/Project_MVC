using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_MVC.Models;
using static Project_MVC.Models.Product;

namespace Project_MVC.Services
{
    public class MySQLProductService : IProductService
    {
        private MyDbContext db = new MyDbContext();
        public bool Create(Product product, ModelStateDictionary state)
        {
            Validate(product, state);
            if (state.IsValid)
            {
                product.ProductCategoryId = Utils.Utility.GetNullableInt(product.ProductCategoryNameAndId.Split(' ')[0]);
                product.ProductCategoryName = product.ProductCategoryNameAndId.Split(' ')[2];
                product.CreatedAt = DateTime.Now;
                product.UpdatedAt = null;
                product.DeletedAt = null;
                db.Products.Add(product);
                db.SaveChanges();
                return true;
            }

            //ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", product.ProductCategoryId);
            return false;
        }

        public bool Delete(Product existProduct, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                existProduct.Status = ProductStatus.Deleted;
                existProduct.DeletedAt = DateTime.Now;
                db.Products.AddOrUpdate(existProduct);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Detail(int? id)
        {
            throw new NotImplementedException();
        }

        public bool Update(Product existProduct, Product product, ModelStateDictionary state)
        {
            if (state.IsValid)
            {
                existProduct.Name = product.Name;
                existProduct.Price = product.Price;
                existProduct.ProductCategoryId = Utils.Utility.GetNullableInt(product.ProductCategoryNameAndId.Split(' ')[0]);
                existProduct.ProductCategoryName = product.ProductCategoryNameAndId.Split(' ')[2];
                existProduct.Description = product.Description;
                existProduct.UpdatedAt = DateTime.Now;
                db.Products.AddOrUpdate(existProduct);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public void Validate(Product product, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(product.ProductCode))
            {
                state.AddModelError("ProductCode", "Product Code is required.");
            }
            var list = db.Products.Where(s => s.ProductCode.Contains(product.ProductCode) && s.Status == ProductStatus.NotDeleted).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("ProductCode", "Product Code already exist.");
            }
        }
    }
}