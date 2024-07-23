using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectPrn.Controller
{
    public class EmailUtil
    {
        private static readonly string fromEmail = "hoapmhe173343@fpt.edu.vn"; // Replace with your email address
        private static readonly string password = "jfis kije snhv wcql"; // Replace with your email password

        public static void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"); // Replace with your email provider's SMTP server
                smtpClient.Port = 587; // Replace with your email provider's SMTP port
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(fromEmail, password);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(fromEmail);
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Sent Successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email. Error: {ex.Message}");
            }
        }
    }
}
