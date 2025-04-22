using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopVista.Models;
using ShopVista.Models.ChatBotModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopVista.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ShopVistaDbContext _context;

        // You'd also have a chat repository or service here

        public ChatController(UserManager<ApplicationUser> userManager, ShopVistaDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var users = _userManager.Users
                .Where(u => u.Id != currentUser.Id && u.EmailConfirmed)
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FullName = $"{u.FirstName} {u.LastName}",
                    Email = u.Email,
                    ProfilePictureUrl = u.ProfilePictureUrl ?? "/images/profiles/default.png"
                })
                .ToList();

            ViewBag.CurrentUserId = currentUser.Id;
            ViewBag.CurrentUserName = $"{currentUser.FirstName} {currentUser.LastName}";

            return View(users);
        }

        public async Task<IActionResult> GetChatHistory(string userId)
        {
            var currentUserId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(currentUserId) || string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user IDs");
            }

            // Fetch messages between these two users
            var messages = await _context.ChatMessages
                .Where(m =>
                    (m.SenderId == currentUserId && m.ReceiverId == userId) ||
                    (m.SenderId == userId && m.ReceiverId == currentUserId))
                .OrderBy(m => m.SentAt)
                .ToListAsync();

            return Ok(messages);
        }
    }
}