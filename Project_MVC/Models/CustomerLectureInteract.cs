using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class CustomerLectureInteract
    {
        [Key]
        public int? Id { get; set; }
        [ForeignKey("Lecture")]
        public int? LectureId { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Lecture Lecture { get; set; }

        #region Rating

        public decimal Rating { get; set; }

        #endregion
    }
}