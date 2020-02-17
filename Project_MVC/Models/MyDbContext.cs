﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    //[DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MyDbContext : IdentityDbContext<AppUser>
    {
        public MyDbContext() : base("name=SQLContext")
        {
            //this.Configuration.ProxyCreationEnabled = false;
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public static MyDbContext Create()
        {
            return new MyDbContext();
        }

        public DbSet<AppRole> IdentityRoles { get; set; }
        public DbSet<LevelOneMenu> LevelOneMenus { get; set; }
        public DbSet<LevelTwoMenu> LevelTwoMenus { get; set; }
        public DbSet<LevelOneProductCategory> LevelOneProductCategories { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<LectureVideo> LectureVideos { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<OwnerOfCourse> OwnerOfCourses { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }
        public DbSet<CustomerLectureInteract> CustomerLectureInteracts { get; set; }
    }
}