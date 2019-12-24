using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_MVC.Models;
using static Project_MVC.Models.Product;

namespace Project_MVC.Services
{
    public class MySQLProductService : ICRUDService<Product>
    {
        private MyDbContext db = new MyDbContext();

        public bool Create(Product item, ModelStateDictionary state)
        {
            throw new NotImplementedException();
        }

        public bool CreateWithImage(Product item, ModelStateDictionary state, IEnumerable<HttpPostedFileBase> images)
        {
            ValidateCode(item, state);
            ValidateCategory(item, state);
            if (state.IsValid)
            {
                //product.ProductCategoryId = Utils.Utility.GetNullableInt(product.ProductCategoryNameAndId.Split(' ')[0]);
                //product.ProductCategoryName = product.ProductCategoryNameAndId.Substring(product.ProductCategoryNameAndId.IndexOf('-') + 2);
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = null;
                item.DeletedAt = null;
                item.Status = ProductStatus.NotDeleted;
                db.Products.Add(item);
                if (images != null)
                {
                    var imageList = new List<ProductImage>();
                    foreach (var image in images)
                    {
                        using (var br = new BinaryReader(image.InputStream))
                        {
                            var data = br.ReadBytes(image.ContentLength);
                            var img = new ProductImage { ProductCode = item.Code };
                            img.SignImage = data;
                            imageList.Add(img);
                        }
                    }
                    item.ProductImages = imageList;
                }
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

        public Product Detail(Product item)
        {
            throw new NotImplementedException();
        }

        public bool Update(Product existProduct, Product product, ModelStateDictionary state)
        {
            //try
            //{
            //    existProduct.Name = product.Name;
            //    existProduct.Price = product.Price;
            //    existProduct.ProductCategoryId = product.ProductCategoryId;
            //    existProduct.ProductCategoryNameAndId = product.ProductCategoryNameAndId;
            //    existProduct.Description = product.Description;
            //    existProduct.UpdatedAt = DateTime.Now;
            //    db.Products.AddOrUpdate(existProduct);
            //    db.SaveChanges();
            //    return true;
            //}
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
            ValidateCategory(product, state);
            if (state.IsValid)
            {
                existProduct.Name = product.Name;
                existProduct.Price = product.Price;
                existProduct.ProductCategoryCode = product.ProductCategoryCode;
                existProduct.Description = product.Description;
                existProduct.UpdatedAt = DateTime.Now;
                db.Products.AddOrUpdate(existProduct);
                db.SaveChanges();

                return true;
            }

            return false;
        }

        public void ValidateCategory(Product product, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(product.ProductCategoryNameAndCode))
            {
                state.AddModelError("ProductCategoryNameAndCode", "Product Category is required.");
            }
        }

        public void ValidateCode(Product product, ModelStateDictionary state)
        {
            if (string.IsNullOrEmpty(product.Code))
            {
                state.AddModelError("Code", "Product Code is required.");
            }
            var list = db.Products.Where(s => s.Code.Contains(product.Code)).ToList();
            if (list.Count != 0)
            {
                state.AddModelError("Code", "Product Code already exist.");
            }
        }
    }
}