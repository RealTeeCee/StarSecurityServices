using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Service
    {
        [Key]
        public long Id { get; set; }

        public string? Image { get; set; }

        public long CategoryId { get; set; }
        //Tạo phương thức ảo ràng buộc FK CategoryId vs Id cua Model Category
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
