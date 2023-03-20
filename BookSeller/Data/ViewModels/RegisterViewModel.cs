using System.ComponentModel.DataAnnotations;

namespace BookSeller.Data.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }


        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password
        {
            get; set;
        }

        [Display(Name ="Xác nhận mật khẩu")]
        [Required(ErrorMessage ="Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Hai mật khẩu không trùng nhau")]
        public string ConfirmPassword
        {
            get; set;
        }
    }
}
