using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace samsungweb.Models
{
    public class ProductColor
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        public string ColorName { get; set; } // VD: "Tím Titanium"

        [Required]
        public string ColorHex { get; set; }  // VD: "#5c5b65" (Mã màu CSS để vẽ chấm tròn)

        public string? ImageUrl { get; set; } // Ảnh điện thoại màu Tím

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}