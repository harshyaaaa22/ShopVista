using Microsoft.EntityFrameworkCore;
using ShopVista.Models;

using System.Threading.Tasks;
using Razorpay.Api;
namespace ShopVista.Service
{
    public interface IPaymentService
    {
        Task<PaymentResponse> InitiatePaymentAsync(string userId, decimal amount, string orderId);
        Task<PaymentResponse> VerifyPaymentAsync(string paymentId, string orderId, string signature);
    }

    public class PaymentService : IPaymentService
    {
        private readonly ShopVistaDbContext _context;
        private readonly ICartService _cartService;
        private readonly string _razorpayKeyId;
        private readonly string _razorpayKeySecret;

        public PaymentService(ShopVistaDbContext context, ICartService cartService, IConfiguration configuration)
        {
            _context = context;
            _cartService = cartService;
            _razorpayKeyId = configuration["Payment:RazorPay:KeyId"];
            _razorpayKeySecret = configuration["Payment:RazorPay:KeySecret"];
        }

        public async Task<PaymentResponse> InitiatePaymentAsync(string userId, decimal amount, string orderId)
        {
            // In a real implementation, you would call the Razorpay API here
            // For demo purposes, we'll create a simulated payment response
            
            var response = new PaymentResponse
            {
                Success = true,
                PaymentId = "pay_" + Guid.NewGuid().ToString("N").Substring(0, 16),
                OrderId = orderId,
                Amount = amount,
                PaymentUrl = $"/Payment/PaymentGateway?orderId={orderId}&amount={amount * 100}"
            };

            // Save payment details in database
            var paymentRecord = new Models.Payment
            {
                OrderId = orderId,
                UserId = userId,
                Amount = amount,
                Status = "initiated",
                CreatedAt = DateTime.UtcNow
            };

            _context.Payments.Add(paymentRecord);
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<PaymentResponse> VerifyPaymentAsync(string paymentId, string orderId, string signature)
        {
            // In a real implementation, you would verify the signature with Razorpay
            // For demo purposes, we'll simulate a successful verification
            
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
            if (payment == null)
            {
                return new PaymentResponse { Success = false, Message = "Payment record not found" };
            }

            payment.Status = "completed";
            payment.PaymentId = paymentId;
            payment.UpdatedAt = DateTime.UtcNow;
            
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            
            return new PaymentResponse 
            { 
                Success = true, 
                PaymentId = paymentId,
                OrderId = orderId,
                Message = "Payment successful"
            };
        }
    }
}