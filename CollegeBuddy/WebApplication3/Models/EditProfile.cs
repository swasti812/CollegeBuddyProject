using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class EditProfile
    {
    
        public string College { get; set; }
       
        public string Branch { get; set; }
       
        public string Year { get; set; }

        public string Password { get; set; }

        public string OldPassword { get; set; }

    }
}