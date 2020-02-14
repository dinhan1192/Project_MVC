﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class OwnerOfCourse
    {
        [Key]
        [DisplayName("Owner Code")]
        public string Code { get; set; }
        [DisplayName("Owner Name")]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public OwnerOfCourseStatus Status { get; set; }
        public enum OwnerOfCourseStatus
        {
            NotDeleted = 0, Deleted = -1
        }
    }
}