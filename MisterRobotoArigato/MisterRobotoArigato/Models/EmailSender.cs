using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MisterRobotoArigato.Models
{
    public class EmailSender : IEmailSender
    {
        IConfiguration Configuration { get; }
       
        public EmailSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// SendGrid email setup
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="htmlMessage"></param>
        /// <returns>no return</returns>
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var msg = new SendGridMessage();

            msg.SetFrom("admin@misterrobotoarigato.com", "Mister Arigato Roboto Admin");

            msg.AddTo(email);
            msg.SetSubject(subject);
            msg.AddContent(MimeType.Html, htmlMessage);

            var client = new SendGridClient(Configuration["API_KEY"]);

            var response = await client.SendEmailAsync(msg);
        }
    }
}