using Microsoft.EntityFrameworkCore;
using ShopVista.Models;

namespace ShopVista.Service
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(string userId);
        Task<CartItem> AddToCartAsync(string userId, int productId, int quantity);
        Task<CartItem> UpdateCartItemAsync(string userId, int cartItemId, int quantity);
        Task RemoveCartItemAsync(string userId, int cartItemId);
        Task ClearCartAsync(string userId);
        
    }

    public class CartService : ICartService
    {
        private readonly ShopVistaDbContext _context;

        public CartService(ShopVistaDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            // Get cart with all items and product details
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                // Create a new cart if one doesn't exist
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<CartItem> AddToCartAsync(string userId, int productId, int quantity)
        {
            var cart = await GetCartAsync(userId);

            // Check if product already exists in cart
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);

            if (existingItem != null)
            {
                // Update quantity if product already in cart
                existingItem.Quantity += quantity;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                // Get product details including price
                var product = await _context.Products.FindAsync(productId);
                if (product == null)
                    throw new Exception("Product not found");

                // Add new item to cart
                existingItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    UnitPrice = product.Price
                };

                _context.CartItems.Add(existingItem);
            }

            // Update cart timestamp
            cart.UpdatedAt = DateTime.UtcNow;
            _context.Carts.Update(cart);

            await _context.SaveChangesAsync();
            return existingItem;
        }

        public async Task<CartItem> UpdateCartItemAsync(string userId, int cartItemId, int quantity)
        {
            var cart = await GetCartAsync(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem == null)
                throw new Exception("Cart item not found");

            if (quantity <= 0)
            {
                await RemoveCartItemAsync(userId, cartItemId);
                return null;
            }

            cartItem.Quantity = quantity;
            cart.UpdatedAt = DateTime.UtcNow;

            _context.CartItems.Update(cartItem);
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return cartItem;
        }

        public async Task RemoveCartItemAsync(string userId, int cartItemId)
        {
            var cart = await GetCartAsync(userId);
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                cart.UpdatedAt = DateTime.UtcNow;
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetCartAsync(userId);

            _context.CartItems.RemoveRange(cart.CartItems);
            cart.UpdatedAt = DateTime.UtcNow;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
