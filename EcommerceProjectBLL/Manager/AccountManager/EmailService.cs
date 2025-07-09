using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EcommerceProjectBLL.Manager.AccountManager
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendPasswordResetEmail(string email, string resetToken)
        {
            try
            {
                var emailSetting = _configuration.GetSection("EmailSettings");
                if (string.IsNullOrEmpty(emailSetting["Email"]))
                    throw new ArgumentNullException("Email is not configured");
                if (string.IsNullOrEmpty(emailSetting["DisplayName"]))
                    throw new ArgumentNullException("Name is not configured");

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    emailSetting["DisplayName"],
                    emailSetting["Email"]));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Reset Your Password";
                message.Body = new TextPart(TextFormat.Html)
                {
                    Text =$@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
            <p>Hello,</p>
            <p>You requested to reset your password.</p>
            <p><strong>Copy this token and paste it in the app:</strong></p>
            <div style='background: #f5f5f5; padding: 15px; font-size: 24px; font-weight: bold; text-align: center;'>
                {resetToken}
            </div>
            <p>This token will expire in 30 minutes.</p>
            <p>If you did not request this, please ignore this email.</p>
            <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'/>
            <p style='font-size: 12px; color: #777;'>© {DateTime.Now.Year} Nada Ashraf. All rights reserved.</p>
        </div>"
                };
                using var smtp = new SmtpClient();
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                await smtp.ConnectAsync(
                    emailSetting["SmtpServer"],
                    int.Parse(emailSetting["SmtpPort"]),
                    SecureSocketOptions.StartTls);

                if (smtp.Capabilities.HasFlag(MailKit.Net.Smtp.SmtpCapabilities.Authentication))
                {
                    await smtp.AuthenticateAsync(emailSetting["Email"], emailSetting["Password"]);
                }

                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation($"Password reset email sent to {email}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send reset email to {email}");
                throw;
            }
        }

        public async Task SendVerificationEmail(string email, string verificationCode)
        {
          
            try
            {
                var emailSetting = _configuration.GetSection("EmailSettings");
                if (string.IsNullOrEmpty(emailSetting["Email"]))
                      throw new ArgumentNullException("Email is not configured");
                if (string.IsNullOrEmpty(emailSetting["DisplayName"]))
                    throw new ArgumentNullException("Name is not configured");

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    emailSetting["DisplayName"],
                    emailSetting["Email"]));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Your Email Verification Code";

                message.Body = new TextPart(TextFormat.Html)
                {
                    Text =$@"<div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                          <h2 style='color: #333;'>Email Verification</h2>
                          <p>Thank you for registering with us. To complete your registration, please enter the following verification code:</p>
                          <div style='background: #f5f5f5; padding: 15px; margin: 20px 0; text-align: center; font-size: 24px; font-weight: bold;'>
                                      {verificationCode}
                          </div>
                          <p>This code will expire in 30 minutes. If you didn't request this, please ignore this email.</p>
                          <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'>
                          <p style='font-size: 12px; color: #777;'>© {DateTime.Now.Year} Nada Ashraf. All rights reserved.</p>
                          </div>"
                };
                using var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                client.Timeout = 30000;
                
                await client.ConnectAsync(
                emailSetting["SmtpServer"],
                int.Parse(emailSetting["SmtpPort"]),
                MailKit.Security.SecureSocketOptions.StartTls);

                if (client.Capabilities.HasFlag(SmtpCapabilities.Authentication))
                {
                    var username = emailSetting["Email"];
                    var password = emailSetting["Password"];
                    await client.AuthenticateAsync(emailSetting["Email"],
                    emailSetting["Password"]);
                }
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                _logger.LogInformation($"Verification email successfully delivered to {email}");
            }
           
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Email delivery failed for recipient: {email}");
                throw;
            }
        }
    }
}
