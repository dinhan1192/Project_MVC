using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class ProductVideo
    {
        public int? Id { get; set; }
        [ForeignKey("Product")]
        public string ProductCode { get; set; }
        [DisplayName("Video")]
        public byte[] VideoData { get; set; }
        public string ContentType { get; set; }
        public virtual Product Product { get; set; }
    }
}