using Microsoft.AspNetCore.Mvc;
using ShopVista.Service;

namespace ShopVista.Controllers
{
    public class ChatbotController : Controller
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost]
        public IActionResult GetResponse([FromBody] string message)
        {
            var response = _chatbotService.GetResponse(message);
            return Json(response);
        }
    }
}
