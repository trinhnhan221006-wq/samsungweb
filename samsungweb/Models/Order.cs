using System.ComponentModel.DataAnnotations;

namespace samsungweb.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Ràng buộc dữ liệu: Bắt buộc khách phải nhập các trường này
        [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        public string Address { get; set; }

        public string? Note { get; set; } // Ghi chú thêm (Có thể bỏ trống)

        public DateTime OrderDate { get; set; } = DateTime.Now; // Tự động lấy giờ hiện tại

        public decimal TotalAmount { get; set; } // Tổng tiền cả đơn

        // Trạng thái đơn hàng (VD: "Đang chờ duyệt", "Đang giao", "Đã hủy")
        public string Status { get; set; } = "Chờ xử lý";

        // Mối quan hệ 1-Nhiều: 1 Đơn hàng có nhiều Chi tiết đơn hàng
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}