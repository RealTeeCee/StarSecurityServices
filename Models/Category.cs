using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [StringLength(255)]
        public string Image { get; set; }
        public long UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [StringLength(255)]
        public string? Name { get; set; }
        [StringLength(255)]
        public string? Slug { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
