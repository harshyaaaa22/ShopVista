using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ShopVista.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"User connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation($"User disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string userId, string message)
        {
            try
            {
                _logger.LogInformation($"Message from {Context.ConnectionId} to {userId}: {message}");

                // In a real app, you'd look up the connection ID for this userId
                // For now, we're using the userId directly (this won't work properly)
                await Clients.User(userId).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
                await Clients.Caller.SendAsync("MessageSent", userId, message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending message: {ex.Message}");
                throw;
            }
        }
    }
}