using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class ProductCategory
    {
        public int? Id { get; set; }
        [DisplayName("Product Category Name")]
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public ProductCategoryStatus Status { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        public enum ProductCategoryStatus
        {
            NotDeleted = 0, Deleted = -1
        }

        internal bool IsDeleted()
        {
            return this.Status == ProductCategoryStatus.Deleted;
        }
    }
}