using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyBooking.Business.Services
{
    public class EmailSender
    {
        public static string SendEmail(string email, string subject, string message)
        {
            var smtp = new SmtpClient();

            MailAddress from = new MailAddress("emailtest33781@gmail.com");
            MailAddress to = new MailAddress(email);
            MailMessage email1 = new MailMessage(from, to);

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential("emailtest33781@gmail.com", "wfwsjzkvupmnipke"); // App password
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            string code = GenCode();
            smtp.Send("emailtest33781@gmail.com", email, "Thông báo của EasyBooking", "Đây là mã xác nhận của bạn: " + code);
            return code;
        }

        public static string GenCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[6];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString.ToString();
        }
    }
}
