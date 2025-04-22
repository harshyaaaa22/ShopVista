using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopVista.Models;
using ShopVista.Service;

namespace ShopVista.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(ICartService cartService, UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _cartService.GetCartAsync(userId);
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart([FromBody] CartItemViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            try
            {
                // Add proper validation
                if (model.ProductId <= 0)
                {
                    throw new ArgumentException("Invalid product ID");
                }

                if (model.Quantity <= 0)
                {
                    throw new ArgumentException("Quantity must be greater than zero");
                }

                await _cartService.AddToCartAsync(userId, model.ProductId, model.Quantity);

                // If it's an AJAX request
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var cart = await _cartService.GetCartAsync(userId);
                    return Json(new
                    {
                        success = true,
                        message = "Item added to cart",
                        cartItemCount = cart.CartItems.Sum(c => c.Quantity)
                    });
                }

                // For regular form submissions
                TempData["SuccessMessage"] = "Item added to cart successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["ErrorMessage"] = $"Error adding item to cart: {ex.Message}";
                return RedirectToAction("Details", "Products", new { id = model.ProductId });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCartItem([FromBody] UpdateCartItemViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            try
            {
                await _cartService.UpdateCartItemAsync(userId, model.CartItemId, model.Quantity);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var cart = await _cartService.GetCartAsync(userId);
                    return Json(new
                    {
                        success = true,
                        message = "Cart updated",
                        cartTotal = cart.CartItems.Sum(ci => ci.UnitPrice * ci.Quantity).ToString("C"),
                        cartItemCount = cart.CartItems.Sum(c => c.Quantity)
                    });
                }

                TempData["SuccessMessage"] = "Cart updated successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["ErrorMessage"] = $"Error updating cart: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCartItem([FromBody] RemoveCartItemViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            try
            {
                await _cartService.RemoveCartItemAsync(userId, model.CartItemId);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var cart = await _cartService.GetCartAsync(userId);
                    return Json(new
                    {
                        success = true,
                        message = "Item removed from cart",
                        cartTotal = cart.CartItems.Sum(ci => ci.UnitPrice * ci.Quantity).ToString("C"),
                        cartItemCount = cart.CartItems.Count
                    });
                }

                TempData["SuccessMessage"] = "Item removed from cart";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = ex.Message });
                }

                TempData["ErrorMessage"] = $"Error removing item: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            var userId = _userManager.GetUserId(User);
            await _cartService.ClearCartAsync(userId);

            TempData["SuccessMessage"] = "Cart cleared successfully";
            return RedirectToAction("Index");
        }
    }
}
