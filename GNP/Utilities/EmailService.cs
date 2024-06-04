﻿using System.Net.Mail;
using System.Net;
using GNP.Configuration;
using Microsoft.Extensions.Options;

namespace GNP.Utilities
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        ApplicationSettings applicationSettings;

        public EmailService(IConfiguration configuration, IOptions<ApplicationSettings> appsettings)
        {
            _configuration = configuration;
            applicationSettings = appsettings.Value;
        }

        public async Task SendMailAsync(string to, string subject, string body)
        {
            //var emailConfig = _configuration.GetSection("EmailConfiguration");
            var emailConfig = applicationSettings.EmailConfiguration;

            var fromAddress = new MailAddress(emailConfig.Username, emailConfig.From);//emailConfig["Username"], emailConfig["From"]);
            var toAddress = new MailAddress(to);
            string fromPassword = emailConfig.Password; //emailConfig["Password"];


            //var smtp = new SmtpClient
            //{
            //    Host = emailConfig["SmtpServer"],
            //    Port = int.Parse(emailConfig["Port"]),
            //    EnableSsl = true,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            //};
            var smtp = new SmtpClient
            {
                Host = emailConfig.SmtpServer,
                Port = emailConfig.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}