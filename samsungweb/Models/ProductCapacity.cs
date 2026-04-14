using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace samsungweb.Models
{
    public class ProductCapacity
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        public string Capacity { get; set; } // VD: "256GB", "512GB", "1TB"

        // Số tiền cộng thêm so với giá gốc của Product
        public decimal PriceIncrement { get; set; } // VD: 3000000 

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}