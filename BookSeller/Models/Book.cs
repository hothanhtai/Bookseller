using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BookSeller.Data.Enums;

namespace BookSeller.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Tên Sách")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Display(Description = "Mô tả")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "Giá")]
        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }

        [Display(Name = "Đường dẫn")]
        [Required(ErrorMessage = "ImageURL is required")]
        public string ImageURL { get; set; }

        [Display(Name = "Hình ảnh")]
        [Required(ErrorMessage = "PictureFile is required")]
        [NotMapped]
        public IFormFile PictureFile { get; set; }

        [Display(Name = "Năm xuất bản")]
        [Required(ErrorMessage = "PublishingYear is required")]
        public int PublishingYear { get; set; }

		[Display(Name = "Thể loại")]
		[Required(ErrorMessage = "Vui lòng chọn thể loại")]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public Category Category { get; set; }
		//relationship-author
		[Display(Name = "Tác giả")]
        [Required(ErrorMessage = "Author is required")]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        //relationship-Publisher
        [Display(Name = "NXB")]
        [Required(ErrorMessage = "Publisher is required")]
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
        public Publisher Publisher { get; set; }
    }
}
