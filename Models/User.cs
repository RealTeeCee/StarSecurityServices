using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public long Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Email { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        [StringLength(255)]
        public string? Password { get; set; }
        
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public long BrandId { get; set; }

    }
}
