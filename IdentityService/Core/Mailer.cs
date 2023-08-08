using Microsoft.Extensions.Options;
using IdentityService.Domain;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityService.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMailerService
    {
        void Send(string subject, string body, params string[] to);
    }
    /// <summary>
    /// 
    /// </summary>
    public class MailerService : IMailerService
    {
        readonly EmailConfig config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public MailerService(IOptions<EmailConfig> config) => this.config = config.Value;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="to"></param>
        public void Send(string subject, string body, params string[] to)
        {
            new Task((data) =>
            {
                (string s, string b) = ((string, string))data;

                var client = new SmtpClient(config.Host, config.Port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(config.Username, config.Password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(config.From, config.Displayname),
                    Body = b,
                    Subject = s,
                    IsBodyHtml = true
                };

                if (to != null && to.Any())
                    foreach (var item in to)
                        mailMessage.To.Add(item);

                client.Send(mailMessage);
            }, (subject, body)).Start();
        }

    }
}
