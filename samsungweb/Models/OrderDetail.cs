namespace samsungweb.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }

        // Khóa ngoại nối về bảng Order
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Khóa ngoại nối về bảng Product (Sản phẩm)
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Lưu lại chính xác thông tin cấu hình khách đã chọn
        public string ProductName { get; set; }
        public string? SelectedColor { get; set; }
        public string? SelectedCapacity { get; set; }

        public int Quantity { get; set; } // Số lượng
        public decimal Price { get; set; } // Đơn giá tại thời điểm mua (Tránh sau này đổi giá web làm đổi luôn lịch sử đơn hàng)
        public decimal Total => Quantity * Price; // Thành tiền của dòng này
    }
}