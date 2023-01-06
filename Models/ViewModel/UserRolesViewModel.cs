using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class UserRolesViewModel
    {
        public string RoleId { get; set; }
        //public string UserId { get; set; } comment reason: in order to not duplicate user Id for every role that add or remove from this user
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
