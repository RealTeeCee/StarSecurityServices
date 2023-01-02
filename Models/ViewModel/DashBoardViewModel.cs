using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class DashBoardViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public List<User> Employees { get; set; }
        public List<User> Managers { get; set; }
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public IEnumerable<Vacancy> Vacancies { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
        public IEnumerable<Branch> Branches { get; set; }
        public IEnumerable<Rating> Ratings { get; set; }
        public IEnumerable<UserDetail> UserDetails { get; set; }
    }
}
