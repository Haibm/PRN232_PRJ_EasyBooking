using EasyBooking.Data.Entities;

namespace EasyBooking.Data.Repositories
{
    public interface IDiscountCodeRepository
    {
        Task<IEnumerable<DiscountCode>> GetAllAsync();
        Task<DiscountCode> GetByIdAsync(int id);
        Task<DiscountCode> GetByCodeAsync(string code);
        Task<IEnumerable<DiscountCode>> GetActiveCodesAsync();
        Task<IEnumerable<DiscountCode>> GetAvailableCodesAsync();
        Task<DiscountCode> AddAsync(DiscountCode discountCode);
        Task<DiscountCode> UpdateAsync(DiscountCode discountCode);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(string code);
        Task<bool> IsCodeValidAsync(string code);
    }
} 