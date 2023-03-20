using System.ComponentModel.DataAnnotations;

namespace BookSeller.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên danh mục")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [Required]
        public string Description { get; set; }
    }
}
