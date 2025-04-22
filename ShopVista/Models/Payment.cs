namespace ShopVista.Models
{
    
        public class Payment
        {
            public int Id { get; set; }
            public string? OrderId { get; set; }
            public string? UserId { get; set; }
            public decimal? Amount { get; set; }
            public string? Status { get; set; }
            public string? PaymentId { get; set; }
            public DateTime? CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
        }

        
    
}
