using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class BranchUsersViewModel
    {
        public string UserId { get; set; }       
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public bool IsSelected { get; set; } //determine if the checkbox is checked
    }
}
