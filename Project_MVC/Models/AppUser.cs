using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Project_MVC.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderStatus Gender { get; set; }
        public DateTime BirthDay { get; set; }
        [NotMapped]
        public bool isChoosen { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [NotMapped]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public string CodeToResetPassword { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public enum GenderStatus
        {
            Male = 1, Female = 0
        }


        public AppUser()
        {

        }
    }
}