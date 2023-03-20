using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace BookSeller.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name ="Họ Tên")]
        public string FullName { get; set; }
    }
}
