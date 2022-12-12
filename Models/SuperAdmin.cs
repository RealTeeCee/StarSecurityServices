using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SuperAdmin
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
        public string? Image { get; set; }

        public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }

        [DefaultValue(1)]
        public byte Status { get; set; } = 1;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
