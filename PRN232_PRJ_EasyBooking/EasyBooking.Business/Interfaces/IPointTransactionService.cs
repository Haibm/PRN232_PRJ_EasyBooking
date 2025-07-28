using EasyBooking.Business.DTOs;

namespace EasyBooking.Business.Interfaces
{
    public interface IPointTransactionService
    {
        Task<IEnumerable<PointTransactionDto>> GetAllAsync();
        Task<PointTransactionDto> GetByIdAsync(int id);
        Task<IEnumerable<PointTransactionDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<PointTransactionDto>> GetByUserIdAndTypeAsync(int userId, string transactionType);
        Task<IEnumerable<PointTransactionDto>> GetByOrderIdAsync(int orderId);
        Task<IEnumerable<PointTransactionDto>> GetByDiscountCodeIdAsync(int discountCodeId);
        Task<PointTransactionDto> AddAsync(PointTransactionDto pointTransactionDto);
        Task<PointTransactionDto> UpdateAsync(PointTransactionDto pointTransactionDto);
        Task DeleteAsync(int id);
        Task<int> GetUserTotalPointsAsync(int userId);
        Task<int> GetUserEarnedPointsAsync(int userId);
        Task<int> GetUserUsedPointsAsync(int userId);
        Task<PointTransactionDto> CreateEarnTransactionAsync(int userId, int points, string description, int? orderId = null, string createBy = "System");
        Task<PointTransactionDto> CreateUseTransactionAsync(int userId, int points, string description, int? orderId = null, string createBy = "System");
        Task<PointTransactionDto> CreateRedeemTransactionAsync(int userId, int points, string description, int discountCodeId, string createBy = "System");
    }
} 