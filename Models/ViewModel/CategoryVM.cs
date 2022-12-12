using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class CategoryVM
    {        

        public CategoryBranch CategoryBranch { get; set; }
        public Category Category { get; set; } 
        public List<Category> Categories {get;set;}
    }
}
