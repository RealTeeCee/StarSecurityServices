using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class UserClaimsViewModel //One-to-many relationship from User to Claims
    {
        public UserClaimsViewModel() //This constructor use to initialize Claims property with new List<> to avoid null reference exception 
        {
            Claims = new List<UserClaim>();
        }

        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; }

    }
}
