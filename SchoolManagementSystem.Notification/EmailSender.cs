using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MailKit.Net.Smtp;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Notification
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("The recipient email address cannot be null or empty.", nameof(email));
            }



            var smtpClient = new SmtpClient();
            try
            {
                var mailMessage = new MimeMessage();
                var fromAddress = _configuration["EmailSettings:SmtpUser"];
                var adminEmail = _configuration["EmailSettings:AdminEmail"];

                if (string.IsNullOrEmpty(fromAddress) || string.IsNullOrEmpty(adminEmail))
                {
                    throw new InvalidOperationException("Email settings are not correctly configured.");
                }

                mailMessage.From.Add(new MailboxAddress("School Management System", fromAddress));
                mailMessage.To.Add(new MailboxAddress("", email));
                mailMessage.Subject = subject;
                mailMessage.Body = new TextPart("plain") { Text = message };

                smtpClient.Connect(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]), true);
                smtpClient.Authenticate(fromAddress, _configuration["EmailSettings:SmtpPass"]);
                await smtpClient.SendAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"An error occurred while sending email: {ex.Message}");
            }
            finally
            {
                await smtpClient.DisconnectAsync(true);
                smtpClient.Dispose();
            }
        }
    }
}
