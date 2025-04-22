using Microsoft.AspNetCore.Identity;

namespace ShopVista.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional user properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; } 
        public string Address { get; set; } = "Not Provided";
        public string ProfilePictureUrl { get; set; } = "Not Provided";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
