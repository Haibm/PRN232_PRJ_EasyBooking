using System.Collections.Generic;
using EasyBooking.Business.DTOs;

namespace EasyBooking.Business.Interfaces
{
    public interface ITicketService
    {
        // ... các hàm khác ...
        IEnumerable<TicketDetailDto> GetByOrderHistoryIdWithDetails(int orderHistoryId);
    }
} 