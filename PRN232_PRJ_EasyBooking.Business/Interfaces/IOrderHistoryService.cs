using System;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Business.Interfaces
{
    public interface IOrderHistoryService
    {
        void Add(OrderHistoryDto orderHistoryDto);
        int GetLatestIdByPaymentAndUser(int paymentId, int userId);
    }
} 