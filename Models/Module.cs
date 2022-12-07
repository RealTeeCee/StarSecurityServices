using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Module
    {
        [Key]
        public long Id { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }
        public string? Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
