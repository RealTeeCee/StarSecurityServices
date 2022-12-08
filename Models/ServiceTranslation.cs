using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ServiceTranslation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ServiceId { get; set; }
        //Tạo phương thức ảo ràng buộc FK ServiceId vs Id cua Model Service
        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }

        public string LanguageId { get; set; }
        //Tạo phương thức ảo ràng buộc FK LanguageId vs Id cua Model Language
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
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
    }
}
