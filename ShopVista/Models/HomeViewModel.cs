namespace ShopVista.Models
{
    public class HomeViewModel
    {
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Product>? FeaturedProducts { get; set; }
        public IEnumerable<Product>? NewArrivals { get; set; }
        public IEnumerable<Testimonial>? Testimonials { get; set; }
        public IEnumerable<Brand>? Brands { get; set; }
    }

    public class ProductDetailsViewModel
    {
        public Product? Product { get; set; }
        public IEnumerable<Product>? RelatedProducts { get; set; }
    }
}
