using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class Lecture
    {
        public int? Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        public decimal? Rating { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public LectureStatus Status { get; set; }
        [ForeignKey("Product")]
        public string ProductCode { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<LectureVideo> LectureVideos { get; set; }
        [NotMapped]
        public string LectureVideoValidation { get; set; }

        public enum LectureStatus
        {
            NotDeleted = 0, Deleted = -1
        }

        internal bool IsDeleted()
        {
            return this.Status == LectureStatus.Deleted;
        }
    }
}