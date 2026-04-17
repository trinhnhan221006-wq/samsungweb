using ĐangNhap.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using samsungweb.Models;
using System.Threading.Tasks;

namespace samsungweb.Controllers
{
    public class LoginController : Controller
    {
        
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ================= ĐĂNG NHẬP =================
        [HttpGet]
        public IActionResult Login () 
        {
            return View("Login"); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    // Đăng nhập thành công -> Về trang chủ
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
            }
            return View("Login", model);
        }

        // ================= ĐĂNG KÝ =================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };

                // Identity sẽ tự động mã hóa Password và lưu vào Database
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Đăng ký xong -> Quay lại form Đăng nhập
                    return RedirectToAction("Login", "Login");
                }

                // Báo lỗi nếu mật khẩu quá yếu (không có chữ hoa, số...)
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // ================= ĐĂNG XUẤT =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Lệnh quan trọng nhất: Xóa sạch Session và Cookie đăng nhập
            await _signInManager.SignOutAsync();

            // Đăng xuất xong thì đưa người dùng về trang chủ Store
            return RedirectToAction("Index", "Home");
        }
    }
}