using Microsoft.AspNetCore.Mvc;
using samsungweb.Data; 
using samsungweb.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace samsungweb.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // 1. Lấy danh sách sản phẩm cho Khối Điện thoại (Quy ước Section = 1)
            // Sắp xếp theo DisplayOrder từ nhỏ đến lớn
            var phones = _context.HomeDisplays
                                 .Include(h => h.Product) // Phải Include để nó kéo theo cả Tên, Ảnh, Giá từ bảng Product sang
                                 .Where(h => h.Section == 1)
                                 .OrderBy(h => h.DisplayOrder)
                                 .Select(h => h.Product) // Lõi HTML đang cần Product, nên mình chỉ trích xuất cục Product ra thôi
                                 .ToList();

            // 2. Lấy danh sách sản phẩm cho Khối TV (Quy ước Section = 2)
            var tvs = _context.HomeDisplays
                              .Include(h => h.Product)
                              .Where(h => h.Section == 2)
                              .OrderBy(h => h.DisplayOrder)
                              .Select(h => h.Product)
                              .ToList();

            // Bỏ khối TV vào "túi dự phòng"
            ViewBag.TvProducts = tvs;

            // Trả khối Điện thoại ra làm Model chính
            return View(phones);
        }


        public async Task<IActionResult> Store(int? categoryId, string? searchString)
        {
            // 1. Kéo Danh mục Cha VÀ lôi kèm toàn bộ Danh mục Con của nó lên (.Include)
            ViewBag.Categories = await _context.Categories
                .Include(c => c.SubCategories)
                .Where(c => c.ParentId == null)
                .ToListAsync();

            var productsQuery = _context.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                // 2. Kéo Danh mục hiện tại, lôi kèm cả Cha và Con của nó
                var currentCat = await _context.Categories
                    .Include(c => c.ParentCategory)
                    .Include(c => c.SubCategories)
                    .FirstOrDefaultAsync(c => c.Id == categoryId);

                ViewBag.CurrentCategory = currentCat;

                if (currentCat != null)
                {
                    var categoryIds = new List<int> { currentCat.Id };
                    // Nếu nó là Cha, lấy thêm ID của tất cả bọn Con
                    if (currentCat.ParentId == null && currentCat.SubCategories != null)
                    {
                        categoryIds.AddRange(currentCat.SubCategories.Select(s => s.Id));
                    }
                    productsQuery = productsQuery.Where(p => categoryIds.Contains(p.CategoryId));
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchString));
                ViewBag.CurrentSearch = searchString;
            }

            return View(await productsQuery.ToListAsync());
        }

        public async Task<IActionResult> CapQuyenAdmin(
        [FromServices] RoleManager<IdentityRole> roleManager,
        [FromServices] UserManager<IdentityUser> userManager)
        {
            // 1. Tạo chức vụ "Admin" trong hệ thống nếu chưa có
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // 2. Tìm tài khoản của bạn (THAY EMAIL CỦA BẠN VÀO ĐÂY NHÉ)
            var user = await userManager.FindByEmailAsync("trinhnhan221006@gmail.com"); // <-- Sửa email này

            if (user != null)
            {
                // 3. Trao ấn kiếm (Gán quyền Admin)
                await userManager.AddToRoleAsync(user, "Admin");
                return Content("Đã cấp quyền Admin thành công rực rỡ! Giờ hãy quay lại đăng nhập.");
            }

            return Content("Không tìm thấy tài khoản này, kiểm tra lại email nhé.");
        }

        [HttpGet]
        public async Task<IActionResult> GetProductDetails(int id)
        {
            // Tìm sản phẩm trong Database dựa vào ID khách hàng click
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(); // Báo lỗi nếu không tìm thấy
            }

            // Trả về dữ liệu nguyên bản dưới dạng JSON để JavaScript đọc
            return Json(new
            {
                id = product.Id,
                name = product.Name,
                price = product.Price.ToString("N0") + " VNĐ", // Định dạng tiền luôn
                imageUrl = product.ImageUrl,
                screenSize = product.ScreenSize,
                cameraInfo = product.CameraInfo,
                chipset = product.Chipset,
                battery = product.Battery
            });
        }

        [HttpGet]
        public IActionResult Buy(int id)
        {
            // Tạm thời truyền ID này sang View để Giai đoạn 3 chúng ta gọi Database
            ViewBag.ProductId = id;
            return View();
        }
    }
}