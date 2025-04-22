using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopVista.Models;

namespace ShopVista.Controllers
{
    public class HomeController : Controller
    {
        private readonly ShopVistaDbContext _context;

        public HomeController(ShopVistaDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new HomeViewModel
            {
                Categories = await _context.Categories.ToListAsync(),
                FeaturedProducts = await _context.Products
                    .Where(p => p.IsHot || p.IsNew)
                    .Take(4)
                    .ToListAsync(),
                NewArrivals = await _context.Products
                    .Where(p => p.IsNew)
                    .Take(4)
                    .ToListAsync(),
                Testimonials = await _context.Testimonials.ToListAsync(),
                Brands = await _context.Brands.ToListAsync()
            };

            return View(viewModel);
        }




        public IActionResult Error()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email is required.");
            }

            var newsletter = new Newsletter { Email = email };
            _context.Newsletters.Add(newsletter);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Successfully subscribed!" });
        }
    }
}
