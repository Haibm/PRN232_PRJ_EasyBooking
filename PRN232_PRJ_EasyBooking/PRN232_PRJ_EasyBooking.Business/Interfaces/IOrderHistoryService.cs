using System;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Business.Interfaces
{
    public interface IOrderHistoryService
    {
        void Add(OrderHistoryDto orderHistoryDto);
    }
} 