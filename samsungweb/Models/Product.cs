using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace samsungweb.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200)]
        public string Name { get; set; } 

        public decimal Price { get; set; } 

        public string? ImageUrl { get; set; } 


        
        public int CategoryId { get; set; }


        [Display(Name = "Màn hình")]
        public string? ScreenSize { get; set; } // VD: 6.8 inch Dynamic AMOLED 2X

        [Display(Name = "Camera")]
        public string? CameraInfo { get; set; } // VD: Hệ thống camera pro 200MP...

        [Display(Name = "Chip xử lý")]
        public string? Chipset { get; set; } // VD: Snapdragon 8 Gen 4

        [Display(Name = "Pin & Sạc")]
        public string? Battery { get; set; } // VD: 5000 mAh, sạc nhanh 45W

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        // --- CÁC DANH SÁCH TÙY CHỌN CHO TRANG MUA HÀNG ---
        public ICollection<ProductColor>? ProductColors { get; set; }
        public ICollection<ProductCapacity>? ProductCapacities { get; set; }

        // Đánh dấu sản phẩm có được hiển thị ra trang chủ hay không
        public bool IsFeatured { get; set; } = false;
    }
}
