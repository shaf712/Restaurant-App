using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace WebApplication2.Areas.Admin.Models
{
    public class User
    {
        public int ID { get; set; }
        
        [Required(ErrorMessage ="Please provide username", AllowEmptyStrings =false)]
        [StringLength(20,MinimumLength =3, ErrorMessage ="Username must be at least 3 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Please provide a password", AllowEmptyStrings =false)]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Password must be at least 5 characters")]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public int Permission { get; set; }
    }
}