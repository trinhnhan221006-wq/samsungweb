using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SamsungWeb.Models;
using samsungweb.Data;

namespace samsungweb.Controllers
{
    public class HomeDisplaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeDisplaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HomeDisplays
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HomeDisplays.Include(h => h.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HomeDisplays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeDisplay = await _context.HomeDisplays
                .Include(h => h.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeDisplay == null)
            {
                return NotFound();
            }

            return View(homeDisplay);
        }

        // GET: HomeDisplays/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: HomeDisplays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Section,DisplayOrder")] HomeDisplay homeDisplay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homeDisplay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", homeDisplay.ProductId);
            return View(homeDisplay);
        }

        // GET: HomeDisplays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeDisplay = await _context.HomeDisplays.FindAsync(id);
            if (homeDisplay == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", homeDisplay.ProductId);
            return View(homeDisplay);
        }

        // POST: HomeDisplays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Section,DisplayOrder")] HomeDisplay homeDisplay)
        {
            if (id != homeDisplay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homeDisplay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomeDisplayExists(homeDisplay.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", homeDisplay.ProductId);
            return View(homeDisplay);
        }

        // GET: HomeDisplays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homeDisplay = await _context.HomeDisplays
                .Include(h => h.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (homeDisplay == null)
            {
                return NotFound();
            }

            return View(homeDisplay);
        }

        // POST: HomeDisplays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homeDisplay = await _context.HomeDisplays.FindAsync(id);
            if (homeDisplay != null)
            {
                _context.HomeDisplays.Remove(homeDisplay);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomeDisplayExists(int id)
        {
            return _context.HomeDisplays.Any(e => e.Id == id);
        }
    }
}
