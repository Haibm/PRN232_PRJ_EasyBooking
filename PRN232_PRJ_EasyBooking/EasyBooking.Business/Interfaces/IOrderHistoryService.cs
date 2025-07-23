using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyBooking.Business.DTOs;
using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Interfaces
{
    public interface IOrderHistoryService
    {
        void Add(OrderHistoryDto orderHistoryDto);
        int GetLatestIdByPaymentAndUser(int paymentId, int userId);
        IEnumerable<OrderHistoryDto> GetByUserId(int userId);
    }
}
