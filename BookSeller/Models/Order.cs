using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookSeller.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [Display(Name = "Tên người nhận")]
        [Required(ErrorMessage = "Không được để trống tên người nhận")]
        public string Name { get; set; }


        [Display(Name = "SĐT người nhận")]
        [Required(ErrorMessage = "Không được để trống SĐT")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Địa chỉ người nhận")]
        [Required(ErrorMessage = "Không được để trống địa chỉ người nhận")]
        public string Address { get; set; }

        public int Status { get; set; }
        public DateTime OrderDate { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public static implicit operator Order(Task<Order?> v)
        {
            throw new NotImplementedException();
        }
    }
}
