using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopVista.Models;

namespace ShopVista.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopVistaDbContext _context;

        public ProductController(ShopVistaDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var shopVistaDbContext = _context.Products.Include(p => p.Category);
            return View(await shopVistaDbContext.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,OldPrice,ImageUrl,CategoryId,Rating,IsNew,IsHot,IsOnSale")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,OldPrice,ImageUrl,CategoryId,Rating,IsNew,IsHot,IsOnSale")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }


        public async Task<IActionResult> Search(ProductSearchViewModel searchModel)
        {
            // Prepare the view model
            var viewModel = new ProductSearchViewModel
            {
                SearchTerm = searchModel.SearchTerm,
                CategoryId = searchModel.CategoryId,
                Categories = await _context.Categories.ToListAsync()
            };

            // Start with a query that includes category
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            // Apply search term filter if provided
            if (!string.IsNullOrWhiteSpace(searchModel.SearchTerm))
            {
                query = query.Where(p =>
                    p.Name.Contains(searchModel.SearchTerm) ||
                    p.Description.Contains(searchModel.SearchTerm)
                );
            }

            // Apply category filter if selected
            if (searchModel.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == searchModel.CategoryId.Value);
            }

            // Get the filtered products
            viewModel.Products = await query.ToListAsync();

            // Prepare category dropdown
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", searchModel.CategoryId);

            return View(viewModel);
        }

        // GET: Product/CategoryProducts/5
        public async Task<IActionResult> CategoryProducts(int categoryId)
        {
            var productsInCategory = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            ViewBag.CategoryName = _context.Categories
                .FirstOrDefault(c => c.Id == categoryId)?.Name;

            return View(productsInCategory);
        }

    }
}
