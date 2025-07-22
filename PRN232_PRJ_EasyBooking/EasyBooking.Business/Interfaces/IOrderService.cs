using EasyBooking.Business.DTOs;
using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Interfaces
{
    public interface IOrderService
    {
        string GenerateCodePayment();
    }
} 