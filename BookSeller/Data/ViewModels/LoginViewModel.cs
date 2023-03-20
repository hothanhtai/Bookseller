using System.ComponentModel.DataAnnotations;

namespace BookSeller.Data.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required]
        [DataType(DataType.Password)]
        public string Password
        {
            get; set;
        }
    }
}
