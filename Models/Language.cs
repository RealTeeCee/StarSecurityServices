﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Language
    {
        public long Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}
