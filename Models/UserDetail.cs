using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class UserDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long UserId { get; set; }
        //Tạo phương thức ảo ràng buộc FK UserId vs Id cua Model User
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [StringLength(20)]
        [Required]
        public string? UserCode { get; set; }
        [Column(TypeName ="nvarchar")]
        [StringLength(255)]
        public string? Education { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        public string? Department { get; set; }

        public byte? Grade { get; set; }

        [StringLength(255)]
        public string? Client { get; set; }

        [StringLength(255)]
        public string? Award { get; set; }


        public DateTime? UpdatedAt { get; set; }
    }
}
