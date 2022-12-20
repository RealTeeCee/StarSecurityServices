using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public static class ClaimsStore
    {       
        public static List<Claim> AllClaims = new List<Claim>() //System.Security.Claim Overloaded constructor 
        {
            new Claim("Create", "Create"),
            new Claim("Edit", "Edit"),
            new Claim("Delete", "Delete")
        };
    }
    //public static class RoleClaimsStore
    //{
    //    public static List<Claim> AllClaims = new List<Claim>() //System.Security.Claim Overloaded constructor 
    //    {            
    //        new Claim("Create", "Create"),
    //        new Claim("Edit", "Edit"),
    //        new Claim("Delete", "Delete")
    //    };
    //}
}
