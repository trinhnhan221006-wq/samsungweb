using samsungweb.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SamsungWeb.Models // Lưu ý: Nếu project của bro tên khác thì sửa chữ SamsungWeb nhé
{
    public class HomeDisplay
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        [Display(Name = "Sản phẩm")]
        public int ProductId { get; set; }

        // Dòng này giúp kết nối sang bảng Products để lấy Tên, Ảnh, Giá...
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vị trí hiển thị")]
        [Display(Name = "Vị trí khối (1: Điện thoại, 2: TV)")]
        public int Section { get; set; }
        // Quy ước ngầm của anh em mình: 
        // Section = 1 sẽ hiện ở khối trên (Di động)
        // Section = 2 sẽ hiện ở khối dưới (TV & AV)

        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; } = 0; // Số càng nhỏ thì càng đứng đầu
    }
}