using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Language
    {
        [Key]
        public long Id { get; set; }

        public string? Name { get; set; }
        public bool IsDefault { get; set; }

    }
}
