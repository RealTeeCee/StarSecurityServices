using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class User : IdentityUser
    {           
        [StringLength(255)]
        public string? Address { get; set; }
        
        [StringLength(255)]
        public string Address { get; set; }
        
        [StringLength(255)]
        public string Image { get; set; }
       
        public long BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branches { get; set; }
        [DefaultValue(0)]
        public int Role { get; set; }
        [DefaultValue(0)]
        public byte Status { get; set; } = 0;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
