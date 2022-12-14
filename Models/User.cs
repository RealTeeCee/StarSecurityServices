using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        [StringLength(255)]
        public string Password { get; set; }
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(255)]
        [Column(TypeName = "nvarchar")]
        public string? Address { get; set; }

        [StringLength(255)]
        public string? Image { get; set; } = "default.jpg";
        [Range(1, int.MaxValue, ErrorMessage = "You must choose a role")]
        public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
     
        [DefaultValue(1)]
        public byte Status { get; set; } = 1;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
    public class Administrator
    {
        public long Id { get; set; }
        public string Name { get; set; }
        
        public string Email { get; set; }

        public string Password { get; set; }
   
        public string Phone { get; set; }

  
        public string? Address { get; set; }

        
        public string? Image { get; set; } = "default.jpg";
        [Range(1, long.MaxValue, ErrorMessage = "You must choose a role")]
        public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [Range(1, long.MaxValue, ErrorMessage ="You must choose a branch")]
        public long BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [DefaultValue(1)]
        public byte Status { get; set; } = 1;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
