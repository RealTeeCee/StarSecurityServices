using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CategoryTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long CategoryId { get; set; }
        //Tạo phương thức ảo ràng buộc FK CategoryId vs Id cua Model Service
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public string LanguageId { get; set; }
        //Tạo phương thức ảo ràng buộc FK LanguageId vs Id cua Model Language
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        public string? Name { get; set; }
        public string? Slug { get; set; }
    }
}
