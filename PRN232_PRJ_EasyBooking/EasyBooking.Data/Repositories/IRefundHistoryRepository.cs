using EasyBooking.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Data.Repositories
{
    public interface IRefundHistoryRepository
    {
        Task<IEnumerable<RefundHistory>> GetAllAsync();
        Task<RefundHistory> GetByIdAsync(int id);
        Task<IEnumerable<RefundHistory>> GetByUserIdAsync(int userId);
        Task<IEnumerable<RefundHistory>> GetByTicketIdAsync(int ticketId);
        Task AddAsync(RefundHistory refundHistory);
        Task UpdateAsync(RefundHistory refundHistory);
        Task DeleteAsync(int id);
    }
} 