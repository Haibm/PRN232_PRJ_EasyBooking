using System.Collections.Generic;
using EasyBooking.Data.Entities;

namespace EasyBooking.Data.Repositories
{
    public interface ITicketRepository
    {
        // ... các hàm khác ...
        IEnumerable<Ticket> GetByOrderHistoryIdWithDetails(int orderHistoryId);
    }
} 