using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class RoleClaimsViewModel
    {
        public RoleClaimsViewModel() //This constructor use to initialize Claims property with new List<> to avoid null reference exception 
        {
            Claims = new List<RoleClaim>();
        }

        public string RoleId { get; set; }
        public List<RoleClaim> Claims { get; set; }
    }
}
