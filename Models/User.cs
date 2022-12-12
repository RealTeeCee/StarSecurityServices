using System.ComponentModel;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        [StringLength(255)]
        public string? Password { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(255)]
        public string Image { get; set; }
    }
}
