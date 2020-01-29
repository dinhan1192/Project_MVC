using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class LevelTwoMenu
    {
        [Key]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DeletedBy { get; set; }
        public LevelTwoStatus Status { get; set; }
        [ForeignKey("LevelOneMenu")]
        public string LevelOneMenuCode { get; set; }
        public virtual LevelOneMenu LevelOneMenu { get; set; }

        public enum LevelTwoStatus
        {
            NotDeleted = 0, Deleted = -1
        }

        internal bool IsDeleted()
        {
            return this.Status == LevelTwoStatus.Deleted;
        }
    }
}