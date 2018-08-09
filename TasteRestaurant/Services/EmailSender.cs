using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace TasteRestaurant.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration) => _configuration = configuration;

        public Task SendEmailAsync(string email, string subject, string message)
        {
            var userEmail = _configuration["AppSecrets:MailUserName"];
            var passwordEmail = _configuration["AppSecrets:MailPassword"];
            SmtpClient client=new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.Credentials=new NetworkCredential(userEmail, passwordEmail);
            client.EnableSsl = true;

            MailMessage mailMessage=new MailMessage();
            mailMessage.From=new MailAddress(userEmail);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;
            client.Send(mailMessage);

            return Task.CompletedTask;
        }
    }
}
