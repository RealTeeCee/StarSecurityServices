using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Rating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string? UserId { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public long? ProjectId { get; set; }
      
        [ForeignKey("ProjectId")]
        public virtual Project? Project { get; set; }

        public int RatingPoint { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
