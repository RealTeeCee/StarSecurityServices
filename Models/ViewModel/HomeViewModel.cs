﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class HomeViewModel
    {
        public List<Category> Categories { get; set; }                
        public List<CategoryBranch> CategoriesBranches { get; set; }
        public List<Project> Projects { get; set; }
    }
}
