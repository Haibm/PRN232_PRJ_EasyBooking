using EasyBooking.Data.Entities;

namespace EasyBooking.Data.Repositories
{
    public interface IPointConfigRepository
    {
        Task<IEnumerable<PointConfig>> GetAllAsync();
        Task<PointConfig> GetByIdAsync(int id);
        Task<PointConfig> GetActiveConfigAsync();
        Task<PointConfig> AddAsync(PointConfig pointConfig);
        Task<PointConfig> UpdateAsync(PointConfig pointConfig);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
} 