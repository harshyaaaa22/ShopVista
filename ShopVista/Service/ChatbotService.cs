using ShopVista.Models.ChatBotModel;

namespace ShopVista.Service
{
    public interface IChatbotService
    {
        ChatbotResponse GetResponse(string message);
    }

    public class ChatbotService : IChatbotService
    {
        private readonly Dictionary<string, List<string>> _responses;

        public ChatbotService()
        {
            _responses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                ["greeting"] = new List<string> {
                    "Hi there! How can I help you today?",
                    "Hello! Welcome to our store. What are you looking for?"
                },
                ["product"] = new List<string> {
                    "We have a wide range of products. You can browse by category or use the search bar.",
                    "Our latest collections are on the homepage. Can I help you find something specific?"
                },
                ["shipping"] = new List<string> {
                    "We offer free shipping on orders over $50. Standard delivery takes 3-5 business days.",
                    "Our standard shipping is $4.99 and takes 3-5 business days. Express shipping is available for $9.99."
                },
                ["returns"] = new List<string> {
                    "Our return policy allows returns within 30 days of purchase.",
                    "You can return any item within 30 days for a full refund or exchange."
                },
                ["payment"] = new List<string> {
                    "We accept all major credit cards, PayPal, and Apple Pay.",
                    "You can pay with credit cards, PayPal, or Apple Pay at checkout."
                },
                ["discount"] = new List<string> {
                    "Use code WELCOME10 for 10% off your first order!",
                    "Subscribe to our newsletter for exclusive discounts and promotions."
                },
                ["contact"] = new List<string> {
                    "You can reach our customer service at support@yourstore.com or call us at 555-123-4567.",
                    "Our customer service team is available Monday-Friday, 9am-5pm EST."
                },
                ["help"] = new List<string> {
                    "I can help with product information, shipping, returns, payments, and more. What do you need assistance with?",
                    "How can I assist you today? I can answer questions about our products, shipping, or returns."
                }
            };
        }

        public ChatbotResponse GetResponse(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return new ChatbotResponse { Message = GetRandomResponse("greeting"), Success = true };
            }

            string lowerMessage = message.ToLower();

            if (lowerMessage.Contains("hi") || lowerMessage.Contains("hello") || lowerMessage.Contains("hey"))
            {
                return new ChatbotResponse { Message = GetRandomResponse("greeting"), Success = true };
            }
            else if (lowerMessage.Contains("product") || lowerMessage.Contains("item") || lowerMessage.Contains("catalog"))
            {
                return new ChatbotResponse { Message = GetRandomResponse("product"), Success = true };
            }
            else if (lowerMessage.Contains("shipping") || lowerMessage.Contains("delivery") || lowerMessage.Contains("ship"))
            {
                return new ChatbotResponse { Message = GetRandomResponse("shipping"), Success = true };
            }
            else if (lowerMessage.Contains("return") || lowerMessage.Contains("refund") || lowerMessage.Contains("exchange"))
            {
                return new ChatbotResponse { Message = GetRandomResponse("returns"), Success = true };
            }
            else if (lowerMessage.Contains("payment") || lowerMessage.Contains("pay") || lowerMessage.Contains("credit card"))
            {
                return new ChatbotResponse { Message = GetRandomResponse("payment"), Success = true };
            }
            else if (lowerMessage.Contains("discount") || lowerMessage.Contains("coupon") || lowerMessage.Contains("promo"))
            {
                return new ChatbotResponse { Message = GetRandomResponse("discount"), Success = true };
            }
            else if (lowerMessage.Contains("contact") || lowerMessage.Contains("support") || lowerMessage.Contains("help"))
            {
                return new ChatbotResponse { Message = GetRandomResponse("contact"), Success = true };
            }
            else
            {
                return new ChatbotResponse
                {
                    Message = "I'm not sure I understand. You can ask about our products, shipping, returns, payment methods, or discounts.",
                    Success = true
                };
            }
        }

        private string GetRandomResponse(string category)
        {
            if (_responses.TryGetValue(category, out var categoryResponses))
            {
                Random rnd = new Random();
                int index = rnd.Next(categoryResponses.Count);
                return categoryResponses[index];
            }

            return "I'm here to help. What would you like to know?";
        }
    }
}

