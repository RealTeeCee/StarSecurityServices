using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Testimonial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string? Name { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string? Title { get; set; }
        [Column(TypeName = "text")]
        [StringLength(255)]
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        public void CopyFromNotNull(Testimonial info)
        {
            this.Id = info.Id;
            if (!string.IsNullOrEmpty(info.Name)) this.Name = info.Name;
            if (!string.IsNullOrEmpty(info.Title)) this.Title = info.Title;
            if (!string.IsNullOrEmpty(info.Description)) this.Description = info.Description;
            if (info.CreatedAt.HasValue) this.CreatedAt = info.CreatedAt;
            if (info.UpdatedAt.HasValue) this.UpdatedAt = info.UpdatedAt;
        }
    }
}
