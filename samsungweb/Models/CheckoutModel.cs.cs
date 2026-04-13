using System.ComponentModel.DataAnnotations;

namespace ĐangNhap.Models
{
    public class CheckoutModel
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        public string PaymentMethod { get; set; }
    }
}