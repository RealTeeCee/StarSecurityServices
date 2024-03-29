﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(100)]
        [Required]
        public string? Name { get; set; }
        [StringLength(255)]
        public string? Slug { get; set; }

        public string? Image { get; set; } = "default.jpg";

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }

        [DefaultValue(1)]
        public byte Status { get; set; } = 1;

        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string? ShortDescription { get; set; }

        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public long CategoryId { get; set; }
        //Tạo phương thức ảo ràng buộc FK CategoryId vs Id cua Model Category
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public virtual Category Category { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }

    public class ServiceViewModel
    {
        public List<Service> Services { get; set; }
        public List<Vacancy> Vacancies { get; set; }
    }
}
