using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Models.ViewModel
{
    public class UserDetailViewModel
    {
        public string UserId { get; set; }
        //Tạo phương thức ảo ràng buộc FK UserId vs Id cua Model User
        public string Email { get; set; }
        public string? Phone { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string UserName { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        [Column(TypeName = "nvarchar")]
        public string? Address { get; set; } = "Some Address";

        [FileExtension]
        public string? Image { get; set; } = "default.jpg";

        [NotMapped]
        public IFormFile ImageUpload { get; set; }

        [StringLength(20)]
        [Required]
        public string? UserCode { get; set; }
        [Column(TypeName = "nvarchar")]
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
