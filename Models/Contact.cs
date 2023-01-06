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

        [StringLength(50)]
        public string Subject { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(400)]
        public string Message { get; set; }
        [Column(TypeName = "text")]

        public string? ReplyMessage { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
    }
}
