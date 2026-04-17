using Microsoft.AspNetCore.Mvc;
using samsungweb.Models;
using samsungweb.Extensions;
using samsungweb.Data; // Bắt buộc phải có dòng này để gọi Database
using System.Text.Json; // Thêm thư viện này để đóng gói JSON
using System.Text;      // Thêm thư viện này để mã hóa chữ

namespace samsungweb.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        // Bơm cả Database và Configuration vào
        public CheckoutController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
        // 2. XỬ LÝ LƯU ĐƠN HÀNG VÀ BẮN WEBHOOK
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> ProcessCheckout(Order order)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("Cart") ?? new List<CartItem>();
            if (cart.Count == 0) return RedirectToAction("Index", "Home");

            // 1. Lưu Order vào Database
            order.OrderDate = DateTime.Now;
            order.TotalAmount = cart.Sum(c => c.Total);
            order.Status = "Đang xử lý";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // Dùng await cho đồng bộ

            // 2. Lưu OrderDetail
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    SelectedColor = item.SelectedColor,
                    SelectedCapacity = item.SelectedCapacity,
                    Quantity = item.Quantity,
                    Price = item.FinalPrice
                };
                _context.OrderDetails.Add(orderDetail);
            }
            await _context.SaveChangesAsync();

            // 3. Xóa Giỏ hàng
            HttpContext.Session.Remove("Cart");

            // ==========================================
            // 🚀 4. BẮN TÍN HIỆU SANG N8N (AUTO EMAIL)
            // ==========================================
            try
            {
                // Lấy link Webhook từ appsettings.json
                string webhookUrl = _configuration["N8nSettings:OrderWebhookUrl"];

                if (!string.IsNullOrEmpty(webhookUrl))
                {
                    using (var httpClient = new HttpClient())
                    {
                        // Đóng gói thông tin khách hàng thành 1 cục Data
                        var payload = new
                        {
                            orderId = order.Id,
                            customerName = order.CustomerName,
                            email = order.Email,
                            totalAmount = order.TotalAmount.ToString("N0") + " đ"
                        };

                        // Chuyển Data sang định dạng JSON và bắn đi (POST)
                        var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                        await httpClient.PostAsync(webhookUrl, jsonContent);
                    }
                }
            }
            catch (Exception ex)
            {
                // Lỡ n8n có sập thì web vẫn chạy bình thường, không báo lỗi cho khách
                Console.WriteLine("Lỗi gửi Webhook: " + ex.Message);
            }

            // 5. Chuyển khách sang trang Cảm ơn
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