﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(255)]
        public string? Name { get; set; }

        [StringLength(255)]
        public string? Slug { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public long ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }

        public int Priority { get; set; } = 0;

        public string? UpdatedBy { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
