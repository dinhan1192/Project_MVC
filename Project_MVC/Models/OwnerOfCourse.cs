using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required]
        public string Occupation { get; set; }
        public string Description { get; set; }
        [DisplayName("Owner Image")]
        public byte[] ImageData { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        [ForeignKey("ProductCategory")]
        public string ProductCategoryCode { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        [DisplayName("Product Category")]
        [NotMapped]
        [RegularExpression(@"^[0-9A-Z]+\s-\s[0-9a-zA-Z\s+ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềếểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+$", ErrorMessage = "Invalid Product Category")]
        public string ProductCategoryNameAndCode { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public OwnerOfCourseStatus Status { get; set; }
        #region Images members
        //public virtual ICollection<ProductImage> ProductImages { get; set; }
        #endregion
        public enum OwnerOfCourseStatus
        {
            NotDeleted = 0, Deleted = -1
        }

        internal bool IsDeleted()
        {
            return this.Status == OwnerOfCourseStatus.Deleted;
        }
    }
}