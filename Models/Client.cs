using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ClientId { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(500)]
        public string? ClientName { get; set; }

        public long? ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        [ValidateNever]
        public virtual Service? Service { get; set; }

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public virtual User? User { get; set; }
    }
}
