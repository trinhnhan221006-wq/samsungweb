using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using samsungweb.Models;

namespace samsungweb.Controllers
{
    // Ổ KHÓA: Chỉ Admin mới được vào trang Quản lý tài khoản này!
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminUsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // Hàm này sẽ đi thu thập tất cả người dùng và chức vụ của họ
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRolesViewModel = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var viewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    // Lấy ra danh sách các chức vụ của user này
                    Roles = await _userManager.GetRolesAsync(user)
                };
                userRolesViewModel.Add(viewModel);
            }

            return View(userRolesViewModel);
        }

        // 1. Hàm hiển thị form chọn quyền (GET)
        [HttpGet]
        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // Đi lấy tất cả chức vụ trong kho nạp vào Menu xổ xuống
            ViewBag.Roles = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");

            return View(user); // Gửi thông tin người dùng ra giao diện
        }

        // 2. Hàm xử lý khi bấm nút "Xác nhận cấp quyền" (POST)
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            if (!string.IsNullOrEmpty(roleName))
            {
                // Nếu người này chưa có quyền đó thì mới cấp
                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    await _userManager.AddToRoleAsync(user, roleName);
                }
            }
            return RedirectToAction("Index"); // Cấp xong thì quay về trang danh sách
        }

    }
}