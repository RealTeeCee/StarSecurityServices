using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ServiceTranslation
    {
        [Key]
        public long Id { get; set; }

        public long ServiceId { get; set; }
        //Tạo phương thức ảo ràng buộc FK ServiceId vs Id cua Model Service
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }

        public string LanguageId { get; set; }
        //Tạo phương thức ảo ràng buộc FK LanguageId vs Id cua Model Language
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        public string? Name { get; set; }
        public string? Slug { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
    }
}
