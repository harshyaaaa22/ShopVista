using System.ComponentModel.DataAnnotations;

namespace ShopVista.Models
{
    public class ProductSearchViewModel
    {
        [Display(Name = "Search Term")]
        public string? SearchTerm { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public List<Product>? Products { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
