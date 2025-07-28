using EasyBooking.Data.Entities;

namespace EasyBooking.Data.Repositories
{
    public interface IPointTransactionRepository
    {
        Task<IEnumerable<PointTransaction>> GetAllAsync();
        Task<PointTransaction> GetByIdAsync(int id);
        Task<IEnumerable<PointTransaction>> GetByUserIdAsync(int userId);
        Task<IEnumerable<PointTransaction>> GetByUserIdAndTypeAsync(int userId, string transactionType);
        Task<IEnumerable<PointTransaction>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<PointTransaction>> GetByDiscountCodeIdAsync(int discountCodeId);
        Task<PointTransaction> AddAsync(PointTransaction pointTransaction);
        Task<PointTransaction> UpdateAsync(PointTransaction pointTransaction);
        Task DeleteAsync(int id);
        Task<int> GetUserTotalPointsAsync(int userId);
        Task<int> GetUserEarnedPointsAsync(int userId);
        Task<int> GetUserUsedPointsAsync(int userId);
    }
} 