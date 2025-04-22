using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace ShopVista.Models
{
    public class ShopVistaDbContext : IdentityDbContext<ApplicationUser>
    {
        public ShopVistaDbContext(DbContextOptions<ShopVistaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }



        public DbSet<Payment> Payments { get; set; } // Added Payment entity

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category configurations
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500);
                entity.Property(e => e.ProductCount)
                    .HasDefaultValue(0);
            });

            // Product configurations
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.Description)
                    .HasMaxLength(1000);
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();
                entity.Property(e => e.OldPrice)
                    .HasColumnType("decimal(18,2)");
                entity.Property(e => e.ImageUrl)
                    .HasMaxLength(500);
                entity.Property(e => e.Rating)
                    .HasColumnType("decimal(3,2)")
                    .HasDefaultValue(0);

                entity.HasOne(p => p.Category)
                    .WithMany()
                    .HasForeignKey(p => p.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Testimonial configurations
            modelBuilder.Entity<Testimonial>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerName)
                    .HasMaxLength(100);
                entity.Property(e => e.CustomerTitle)
                    .HasMaxLength(200);
                entity.Property(e => e.TestimonialText)
                    .HasMaxLength(1000);
                entity.Property(e => e.AvatarUrl)
                    .HasMaxLength(500);
            });

            // Brand configurations
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.LogoUrl)
                    .HasMaxLength(500);
            });

            // Newsletter configurations
            modelBuilder.Entity<Newsletter>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.HasIndex(e => e.Email)
                    .IsUnique();
                entity.Property(e => e.SubscriptionDate)
                    .HasDefaultValueSql("GETUTCDATE()");
            });

            // Identity table renaming
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
            });
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable("Roles");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            // Cart relationships
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.OrderId)
                    .HasMaxLength(100)
                    .IsRequired(false); // Allows NULL values

                entity.Property(e => e.UserId)
                    .IsRequired(false); // Allows NULL values

                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18,2)")
                    .IsRequired(false); // Allows NULL values

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsRequired(false); // Allows NULL values

                entity.Property(e => e.PaymentId)
                    .HasMaxLength(100)
                    .IsRequired(false); // Allows NULL values

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETUTCDATE()")
                    .IsRequired(false); // Allows NULL values

                entity.Property(e => e.UpdatedAt)
                    .IsRequired(false); // Allows NULL values
            });


            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.SentAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsRead).HasDefaultValue(false);
            });

        }
    }


}
