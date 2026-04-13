using ĐangNhap.Models;
using Microsoft.AspNetCore.Mvc;


namespace ĐangNhap.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            var model = new CheckoutModel
            {
                ProductName = "Laptop Dell",
                Price = 15000000
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(CheckoutModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Thanh toán thành công!";
            }

            return View(model);
        }
    }
}