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
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Email { get; set; }       
        [StringLength(15)]
        public string Phone { get; set; }      

        public long CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public long BranchId { get; set; }
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public string Noted { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
