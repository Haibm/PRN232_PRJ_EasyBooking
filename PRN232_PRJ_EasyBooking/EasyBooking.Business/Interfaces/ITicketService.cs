using EasyBooking.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Business.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task<TicketDto> GetByIdAsync(int id);
        Task UpdateAsync(TicketDto ticketDto);
        Task DeleteAsync(int id);
        TicketDto CreateTicket(TicketDto ticketDto);
        IEnumerable<TicketDetailDto> GetByOrderHistoryIdWithDetails(int orderHistoryId);
        IEnumerable<TicketDetailDto> GetAllByOrderHistoryIdWithDetails(int orderHistoryId);
        Task<IEnumerable<TicketDto>> GetByShowtimeIdAsync(int showtimeId);
        Task<bool> RefundTicketAsync(int ticketId, decimal refundAmount, string refundReason, int userId);
    }
}