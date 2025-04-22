using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopVista.Models;
using ShopVista.Service;
using System.Security.Claims;

namespace ShopVista.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ICartService _cartService;

        public PaymentController(IPaymentService paymentService, ICartService cartService)
        {
            _paymentService = paymentService;
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> InitiatePayment()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _cartService.GetCartAsync(userId);

            if (cart.CartItems.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }

            // Calculate total amount
            decimal totalAmount = cart.CartItems.Sum(i => i.UnitPrice * i.Quantity);

            // Generate an order ID
            string orderId = "order_" + Guid.NewGuid().ToString("N").Substring(0, 16);

            // Initiate payment
            var response = await _paymentService.InitiatePaymentAsync(userId, totalAmount, orderId);

            if (response.Success)
            {
                // Store order ID in session for verification later
                HttpContext.Session.SetString("CurrentOrderId", orderId);

                // Redirect to the payment gateway page
                return Redirect(response.PaymentUrl);
            }

            TempData["ErrorMessage"] = "Failed to initiate payment: " + response.Message;
            return RedirectToAction("Index", "Cart");
        }

        // This action will display the demo payment gateway UI
        public IActionResult PaymentGateway(string orderId, decimal amount)
        {
            var sessionOrderId = HttpContext.Session.GetString("CurrentOrderId");
            if (string.IsNullOrEmpty(sessionOrderId) || sessionOrderId != orderId)
            {
                return RedirectToAction("Index", "Cart");
            }

            ViewBag.OrderId = orderId;
            ViewBag.Amount = amount / 100; // Convert back from paisa to rupees
            ViewBag.RazorpayKeyId = "rzp_test_yourkeyhere"; // Use a test key for demo

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPayment(string paymentId, string orderId, string signature)
        {
            var sessionOrderId = HttpContext.Session.GetString("CurrentOrderId");
            if (string.IsNullOrEmpty(sessionOrderId) || sessionOrderId != orderId)
            {
                return Json(new { success = false, message = "Invalid order" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _paymentService.VerifyPaymentAsync(paymentId, orderId, signature);

            if (response.Success)
            {
                // Clear the cart after successful payment
                await _cartService.ClearCartAsync(userId);

                // Clear the order ID from session
                HttpContext.Session.Remove("CurrentOrderId");

                return Json(new { success = true, message = "Payment successful", redirectUrl = "/Orders/Details/" + orderId });
            }

            return Json(new { success = false, message = response.Message });
        }
    }
}