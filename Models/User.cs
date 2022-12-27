using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class User : IdentityUser
    {
        public string? Phone { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        [Column(TypeName = "nvarchar")]
        public string? Address { get; set; } = "Some Address";

        public string? Image { get; set; } = "default.jpg";

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }

        [DefaultValue(1)]
        public byte Status { get; set; } = 1;
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
   
}
