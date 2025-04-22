using System.ComponentModel.DataAnnotations;

namespace ShopVista.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductCount { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? OldPrice { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public decimal Rating { get; set; }
        public bool IsNew { get; set; }
        public bool IsHot { get; set; }
        public bool IsOnSale { get; set; }
    }

    public class Testimonial
    {
        public int Id { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerTitle { get; set; }
        public string? TestimonialText { get; set; }
        public string? AvatarUrl { get; set; }
    }

    public class Brand
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? LogoUrl { get; set; }
    }

    public class Newsletter
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        public DateTime SubscriptionDate { get; set; } = DateTime.UtcNow;
    }
}
