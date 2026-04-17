using Microsoft.AspNetCore.Mvc;
using samsungweb.Models;
using samsungweb.Extensions;
using samsungweb.Data; // Bắt buộc phải có dòng này để gọi Database

namespace samsungweb.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Bơm Database vào Controller
        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1. HIỂN THỊ TRANG ĐIỀN THÔNG TIN
        // ==========================================
        [HttpGet]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (cart.Count == 0) return RedirectToAction("Index", "Home");

            ViewBag.Cart = cart;
            ViewBag.Total = cart.Sum(c => c.Total);

            return View();
        }

        // ==========================================
        // 2. XỬ LÝ LƯU ĐƠN HÀNG VÀO DATABASE
        // ==========================================
        [HttpPost]
        public IActionResult ProcessCheckout(Order order)
        {
            // 1. Lấy lại giỏ hàng từ Session
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (cart.Count == 0) return RedirectToAction("Index", "Home");

            // 2. Tính toán và bổ sung thông tin tự động cho bảng Order
            order.OrderDate = DateTime.Now;
            order.TotalAmount = cart.Sum(c => c.Total); // Tự tính lại tổng tiền cho an toàn
            order.Status = "Đang xử lý";

            // 3. Bấm nút "Lưu" bảng Order vào CSDL
            _context.Orders.Add(order);
            _context.SaveChanges(); // Lưu xong, CSDL sẽ tự cấp cho đơn hàng một cái ID (order.Id)

            // 4. Bóc từng món trong Giỏ hàng nhét vào bảng OrderDetail
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id, // Móc nối với ID của đơn hàng vừa tạo ở trên
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    SelectedColor = item.SelectedColor,
                    SelectedCapacity = item.SelectedCapacity,
                    Quantity = item.Quantity,
                    Price = item.FinalPrice
                };
                _context.OrderDetails.Add(orderDetail);
            }

            // 5. Bấm "Lưu" toàn bộ chi tiết vào CSDL
            _context.SaveChanges();

            // 6. Xóa sạch Giỏ hàng (Session) sau khi khách đã mua xong
            HttpContext.Session.Remove("Cart");

            // 7. Chuyển khách sang trang Cảm ơn
            return RedirectToAction("Success");
        }

        // ==========================================
        // 3. TRANG BÁO THÀNH CÔNG
        // ==========================================
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}