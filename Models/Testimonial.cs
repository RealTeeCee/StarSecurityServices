using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Testimonial
    {
        [Key]
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
