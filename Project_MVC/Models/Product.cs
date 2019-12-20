using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class Product
    {
        public int? Id { get; set; }
        [DisplayName("Product Code")]
        public string ProductCode { get; set; }
        [DisplayName("Product Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ProductStatus Status { get; set; }
        public int? ProductCategoryId { get; set; }
        //public virtual ProductCategory ProductCategory { get; set; }
        [DisplayName("Product Category")]
        public string ProductCategoryName { get; set; }
        [Required]
        [DisplayName("Name And Id of Product Category")]
        public string ProductCategoryNameAndId { get; set; }
        public enum ProductStatus
        {
            NotDeleted = 0, Deleted = -1
        }

        internal bool IsDeleted()
        {
            return this.Status == ProductStatus.Deleted;
        }
    }
}