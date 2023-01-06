using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Session
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string UserId { get; set; }
        //Tạo phương thức ảo ràng buộc FK UserId vs Id cua Model User
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [StringLength(50)]
        public string? IpAddress { get; set; }
        [Column(TypeName = "text")]
        [StringLength(255)]
        public string? LoginDevice { get; set; }
        public int? LastActivity { get; set; }
    }
}
