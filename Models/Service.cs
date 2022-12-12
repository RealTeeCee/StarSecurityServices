using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }        
        [StringLength(255)]
        public string? Image { get; set; }

        public long CategoryId { get; set; }
        //Tạo phương thức ảo ràng buộc FK CategoryId vs Id cua Model Category
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string? Name { get; set; }
        [StringLength(255)]
        public string? Slug { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string? ShortDescription { get; set; }
        [Column(TypeName = "text")]
        [StringLength(255)]
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
