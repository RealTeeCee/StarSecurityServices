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
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }
        [Required]
        [RegularExpression(@"^[^\s\,]+$", ErrorMessage = "Username Cannot Have WhiteSpaces")]
        public string UserName { get; set; }
        [Required]
        public string Name { get; set; }

        [Required] [EmailAddress]        
        [ValidEmailDomain(allowedDomain: "starsec.com", ErrorMessage = "Email domain must be starsec.com")] //Custom validate 
        public string Email { get; set; }
        public string Address { get; set; }
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
