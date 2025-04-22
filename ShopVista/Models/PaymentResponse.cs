namespace ShopVista.Models
{
    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
        public string PaymentUrl { get; set; }
    }
}
