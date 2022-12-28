using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(255)]        
        public string Name { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        
        public string? Latitude { get; set; }

        public string? Longitude { get; set; }

        public string? Image { get; set; }
        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
        public string? Address { get; set; }
        
        [StringLength(100)]
        public string? Email { get; set; }
        [StringLength(15)]
        public string? Phone { get; set; }
        [StringLength(100)]
        public string? TimeOpen { get; set; }
        [StringLength(255)]
        public string? Facebook { get; set; }
        [StringLength(255)]
        public string? Twitter { get; set; }
        [StringLength(255)]
        public string? Youtube { get; set; }
        [StringLength(255)]
        public string? Instagram { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
