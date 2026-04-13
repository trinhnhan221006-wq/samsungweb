using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using samsungweb.Data;

namespace samsungweb.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CategoryMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy tất cả danh mục, mỗi danh mục lấy kèm theo 4 sản phẩm mới nhất
            var categories = await _context.Categories
                .Include(c => c.Products)
                .ToListAsync();

            return View(categories);
        }
    }
}