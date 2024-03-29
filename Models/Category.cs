﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string? Name { get; set; }
        
        [StringLength(255)]
        public string? Slug { get; set; }

        public string Image { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }

        [StringLength(150)]
        [Column(TypeName = "nvarchar")]
        public string? ShortDescription { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
       
    }

    public class CategoryViewModel
    {
        public List<Category> Categories { get; set; }
        public List<CategoryBranch> CategoriesBranches { get; set; }
        public List<Branch> Branches { get; set; }
        public Branch Branch { get; set; }
    }
}
