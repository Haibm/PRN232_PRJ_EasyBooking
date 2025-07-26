using EasyBooking.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Business.Interfaces
{
    public interface IRefundHistoryService
    {
        Task<IEnumerable<RefundHistoryDto>> GetAllAsync();
        Task<RefundHistoryDto> GetByIdAsync(int id);
        Task<IEnumerable<RefundHistoryDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<RefundHistoryDto>> GetByTicketIdAsync(int ticketId);
        Task AddAsync(RefundHistoryDto refundHistoryDto);
        Task UpdateAsync(RefundHistoryDto refundHistoryDto);
        Task DeleteAsync(int id);
    }
} 