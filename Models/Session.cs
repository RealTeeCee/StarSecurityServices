using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Session
    {
        [Key]
        public long Id { get; set; }

        public long UserId { get; set; }
        //Tạo phương thức ảo ràng buộc FK UserId vs Id cua Model User
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [StringLength(50)]
        public string? IpAddress { get; set; }
        public string? LoginDevice { get; set; }
        public int? LastActivity { get; set; }
    }
}
