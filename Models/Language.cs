using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Language
    {
        [Key]
        [StringLength(5)]
        public string Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        public bool IsDefault { get; set; }

    }
}
