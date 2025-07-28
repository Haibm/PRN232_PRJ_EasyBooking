using EasyBooking.Business.DTOs;

namespace EasyBooking.Business.Interfaces
{
    public interface IPointConfigService
    {
        Task<IEnumerable<PointConfigDto>> GetAllAsync();
        Task<PointConfigDto> GetByIdAsync(int id);
        Task<PointConfigDto> GetActiveConfigAsync();
        Task<PointConfigDto> AddAsync(PointConfigDto pointConfigDto);
        Task<PointConfigDto> UpdateAsync(PointConfigDto pointConfigDto);
        Task DeleteAsync(int id);
        Task DeleteAsync(int id, string username);
        Task<bool> ExistsAsync(int id);
        Task<int> CalculatePointsAsync(decimal amount);
    }
} 