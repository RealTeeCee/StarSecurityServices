using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse", controller:"Account")] //the model in this RegisterViewModel and the Email UI is bound to this Email property
        [ValidEmailDomain(allowedDomain: "starsec.com", ErrorMessage = "Email domain must be starsec.com")] //Custom validate 
        public string Email { get; set; } 
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        
    }
}
