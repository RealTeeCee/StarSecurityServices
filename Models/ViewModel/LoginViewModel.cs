﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username Or Email")]
        public string UserNameOrEmail { get; set; }
        [Required]        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Remember me")]
        
        public bool RememberMe { get; set; }
    }
}
