using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string? Name { get; set; }
        
        [StringLength(255)]
        public string? Slug { get; set; }

        [FileExtension]
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile ImageUpload { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
       
    }
}
