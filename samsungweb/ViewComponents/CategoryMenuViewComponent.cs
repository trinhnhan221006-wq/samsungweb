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
            var categories = await _context.Categories
        .Include(c => c.SubCategories) // Lôi danh mục con lên
        .Include(c => c.Products)      // <--- Bắt buộc phải có dòng này để cat.Products hết bị null!
        .Where(c => c.ParentId == null)
        .ToListAsync();

            return View(categories);
        }
    }
}