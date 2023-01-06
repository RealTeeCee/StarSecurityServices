using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CategoryBranch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long CategoryId { get; set; }
        //Tạo phương thức ảo ràng buộc FK CategoryId vs Id cua Model Service
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public long BranchId { get; set; }
        //Tạo phương thức ảo ràng buộc FK BranchId vs Id cua Model Language
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
 
    }
    public class ViewCategoryBranch
    {             
        public List<Category> Categories { get; set; }
        public List<Branch> Branches { get; set; }
        public long[] CategoryId { get; set; }
        public long BranchId { get; set; }
        
        public ViewCategoryBranch()
        {
            this.Categories = new List<Category>();
            this.Branches = new List<Branch>();
        }        
    }

}

