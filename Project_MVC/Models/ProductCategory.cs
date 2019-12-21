using Newtonsoft.Json;
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
        [Key]
        [DisplayName("Product Category Code")]
        [RegularExpression(@"^[A-Z]{1,3}\d{1,4}$", ErrorMessage = "Invalid Code")]
        public string Code { get; set; }
        [DisplayName("Product Category Name")]
        [Required]
        [RegularExpression(@"^[0-9a-zA-Z\s+ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$", ErrorMessage = "Invalid Product Category")]
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