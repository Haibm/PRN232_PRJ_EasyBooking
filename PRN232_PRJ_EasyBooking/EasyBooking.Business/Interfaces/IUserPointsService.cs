using EasyBooking.Business.DTOs;

namespace EasyBooking.Business.Interfaces
{
    public interface IUserPointsService
    {
        Task<IEnumerable<UserPointsDto>> GetAllAsync();
        Task<UserPointsDto> GetByIdAsync(int id);
        Task<UserPointsDto> GetByUserIdAsync(int userId);
        Task<UserPointsDto> AddAsync(UserPointsDto userPointsDto);
        Task<UserPointsDto> UpdateAsync(UserPointsDto userPointsDto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int userId);
        Task<int> GetUserCurrentPointsAsync(int userId);
        Task<int> GetUserTotalEarnedPointsAsync(int userId);
        Task<int> GetUserTotalUsedPointsAsync(int userId);
        Task<UserPointsDto> InitializeUserPointsAsync(int userId, string createBy = "System");
        Task<UserPointsDto> AddPointsAsync(int userId, int points, string description, int? orderId = null, string createBy = "System");
        Task<UserPointsDto> UsePointsAsync(int userId, int points, string description, int? orderId = null, string createBy = "System");
    }
} 