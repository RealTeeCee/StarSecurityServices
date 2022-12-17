﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        //public string RoleId { get; set; } comment reason: in order to not duplicate role Id for every user that add or remove from this role 
        public string UserName { get; set; }
        public bool IsSelected { get; set; } //determine if the checkbox is checked
    }
}
