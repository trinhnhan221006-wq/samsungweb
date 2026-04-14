namespace samsungweb.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; }

        // Cấu hình khách đã chọn
        public string SelectedColor { get; set; }
        public string SelectedCapacity { get; set; }

        // Giá sau cùng (Đã cộng dung lượng/dịch vụ)
        public decimal FinalPrice { get; set; }

        public int Quantity { get; set; } // Số lượng mua

        // Tính tổng tiền của dòng này (Phòng trường hợp mua 2 cái S26 Ultra)
        public decimal Total => Quantity * FinalPrice;
    }
}