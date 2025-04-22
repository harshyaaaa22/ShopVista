using System.Net.Mail;
using System.Net;

namespace ShopVista.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendConfirmationEmailAsync(string email, string confirmationLink);
        Task SendPasswordResetEmailAsync(string email, string resetLink);
        Task SendTemporaryPasswordEmailAsync(string email, string temporaryPassword);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;
        private readonly string _senderEmail;
        private readonly string _senderName;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpServer = _configuration["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"];
            _senderEmail = _configuration["EmailSettings:SenderEmail"];
            _senderName = _configuration["EmailSettings:SenderName"];
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_senderEmail, _senderName),
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                client.EnableSsl = true;

                await client.SendMailAsync(mailMessage);
            }
        }

        public async Task SendConfirmationEmailAsync(string email, string confirmationLink)
        {
            string subject = "Confirm Your ShopVista Account";
            string message = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #3498db; color: white; padding: 15px; text-align: center; }}
                        .content {{ padding: 20px; }}
                        .button {{ display: inline-block; background-color: #3498db; color: white; text-decoration: none; padding: 10px 20px; border-radius: 4px; }}
                        .footer {{ text-align: center; margin-top: 30px; font-size: 12px; color: #777; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Welcome to ShopVista!</h1>
                        </div>
                        <div class='content'>
                            <p>Thank you for registering with ShopVista. Please confirm your email address to activate your account.</p>
                            <p style='text-align: center;'>
                                <a href='{confirmationLink}' class='button'>Confirm Email Address</a>
                            </p>
                            <p>If you didn't create an account, you can safely ignore this email.</p>
                            <p>If the button above doesn't work, copy and paste the following link into your browser:</p>
                            <p>{confirmationLink}</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; {DateTime.Now.Year} ShopVista. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(email, subject, message);
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            string subject = "Reset Your ShopVista Password";
            string message = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #3498db; color: white; padding: 15px; text-align: center; }}
                        .content {{ padding: 20px; }}
                        .button {{ display: inline-block; background-color: #3498db; color: white; text-decoration: none; padding: 10px 20px; border-radius: 4px; }}
                        .footer {{ text-align: center; margin-top: 30px; font-size: 12px; color: #777; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Password Reset Request</h1>
                        </div>
                        <div class='content'>
                            <p>We received a request to reset your ShopVista account password. Click the button below to create a new password:</p>
                            <p style='text-align: center;'>
                                <a href='{resetLink}' class='button'>Reset Password</a>
                            </p>
                            <p>If you didn't request a password reset, you can safely ignore this email.</p>
                            <p>If the button above doesn't work, copy and paste the following link into your browser:</p>
                            <p>{resetLink}</p>
                            <p>This link will expire in 24 hours.</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; {DateTime.Now.Year} ShopVista. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(email, subject, message);
        }

        public async Task SendTemporaryPasswordEmailAsync(string email, string temporaryPassword)
        {
            string subject = "Your Temporary ShopVista Password";
            string message = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #3498db; color: white; padding: 15px; text-align: center; }}
                        .content {{ padding: 20px; }}
                        .password-box {{ background-color: #f8f9fa; border: 1px solid #ddd; padding: 15px; text-align: center; font-size: 18px; margin: 15px 0; }}
                        .footer {{ text-align: center; margin-top: 30px; font-size: 12px; color: #777; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Your Temporary Password</h1>
                        </div>
                        <div class='content'>
                            <p>You have requested a temporary password for your ShopVista account. You can use this password to log in:</p>
                            <div class='password-box'>
                                <strong>{temporaryPassword}</strong>
                            </div>
                            <p>For security reasons, we recommend changing this password after logging in.</p>
                            <p>If you didn't request a new password, please contact customer support immediately.</p>
                        </div>
                        <div class='footer'>
                            <p>&copy; {DateTime.Now.Year} ShopVista. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";

            await SendEmailAsync(email, subject, message);
        }
    }
}
