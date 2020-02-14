using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class LectureVideo
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Video")]
        public byte[] VideoData { get; set; }
        public string ContentType { get; set; }
        [ForeignKey("Lecture")]
        public int? LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string CreatedBy { get; set; }

        #region Rating

        public int Rating { get; set; }
        public int TotalRaters { get; set; }
        public double AverageRating { get; set; }

        #endregion
    }
}