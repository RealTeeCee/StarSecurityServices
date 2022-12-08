using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Module
    {
        public long Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(255)]
        public string Title { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }


    }
}
