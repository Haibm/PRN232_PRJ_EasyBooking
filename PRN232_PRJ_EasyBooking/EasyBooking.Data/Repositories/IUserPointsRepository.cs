using EasyBooking.Data.Entities;

namespace EasyBooking.Data.Repositories
{
    public interface IUserPointsRepository
    {
        Task<IEnumerable<UserPoints>> GetAllAsync();
        Task<UserPoints> GetByIdAsync(int id);
        Task<UserPoints> GetByUserIdAsync(int userId);
        Task<UserPoints> AddAsync(UserPoints userPoints);
        Task<UserPoints> UpdateAsync(UserPoints userPoints);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int userId);
    }
} 