using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class ProductImage
    {
        public int? Id { get; set; }
        [ForeignKey("Product")]
        public string ProductCode { get; set; }
        [DisplayName("Image")]
        public byte[] ImageData { get; set; }
        public virtual Product Product { get; set; }
    }
}