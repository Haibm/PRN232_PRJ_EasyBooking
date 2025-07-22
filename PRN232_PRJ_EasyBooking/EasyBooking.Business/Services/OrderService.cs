using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Services
{
    public class OrderService : IOrderService
    {
        public string GenerateCodePayment()
        {
            return "DE" + GenerateUniqueId();
        }

        public  string GenerateUniqueId()
        {
            // You can customize this function according to your requirements
            // For simplicity, this example generates a random 8-character string
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] charArray = new char[8];
            Random random = new Random();

            for (int i = 0; i < 8; i++)
            {
                charArray[i] = chars[random.Next(chars.Length)];
            }

            string uniqueId = new string(charArray);
            return uniqueId;
        }
    }
} 