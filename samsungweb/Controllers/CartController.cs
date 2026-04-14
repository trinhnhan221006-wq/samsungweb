using Microsoft.AspNetCore.Mvc;
using samsungweb.Data; // Chỉnh lại theo đúng nơi chứa DbContext của bạn
using samsungweb.Models;
using samsungweb.Extensions;

namespace samsungweb.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. HIỂN THỊ TRANG GIỎ HÀNG
        // ==========================================
        [HttpGet]
        public IActionResult Index()
        {
            // Lấy túi dạ quang (Session) ra xem có gì bên trong không
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            return View(cart);
        }

        // ==========================================
        // 2. XỬ LÝ KHI BẤM "THÊM VÀO GIỎ" TỪ GIAO DIỆN
        // ==========================================
        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItem incomingItem)
        {
            // Bảo mật Back-end: Lấy ID do JS gửi lên, chui vào DB tìm tên và ảnh thật
            // (Tuyệt đối không để hacker F12 đổi tên sản phẩm gửi lên)
            var product = _context.Products.FirstOrDefault(p => p.Id == incomingItem.ProductId);

            if (product == null) return BadRequest("Sản phẩm không tồn tại!");

            // Cập nhật thông tin chuẩn xác từ Database
            incomingItem.ProductName = product.Name;
            incomingItem.ImageUrl = product.ImageUrl ?? "default.jpg";
            incomingItem.Quantity = 1; // Mặc định mỗi lần bấm là thêm 1 cái

            // Kéo giỏ hàng hiện tại ra
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            // Kiểm tra: Nếu khách chọn đúng cái máy đó, cùng màu, cùng dung lượng -> Chỉ tăng số lượng
            var existingItem = cart.FirstOrDefault(c =>
                c.ProductId == incomingItem.ProductId &&
                c.SelectedColor == incomingItem.SelectedColor &&
                c.SelectedCapacity == incomingItem.SelectedCapacity);

            if (existingItem != null)
            {
                existingItem.Quantity += 1;
            }
            else
            {
                cart.Add(incomingItem); // Nếu cấu hình mới tinh thì thêm dòng mới
            }

            // Gói gọn cất lại vào Session
            HttpContext.Session.Set("Cart", cart);

            return Ok(new { success = true });
        }

        // ==========================================
        // 3. XÓA MÓN HÀNG KHỎI GIỎ
        // ==========================================
        [HttpGet]
        public IActionResult Remove(int id, string color, string capacity)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();

            var itemToRemove = cart.FirstOrDefault(c =>
                c.ProductId == id && c.SelectedColor == color && c.SelectedCapacity == capacity);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                HttpContext.Session.Set("Cart", cart); // Lưu lại sau khi xóa
            }

            return RedirectToAction("Index"); // Tải lại trang Giỏ hàng
        }
    }
}