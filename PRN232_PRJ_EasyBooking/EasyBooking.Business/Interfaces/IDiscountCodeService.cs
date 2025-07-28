using EasyBooking.Business.DTOs;

namespace EasyBooking.Business.Interfaces
{
    public interface IDiscountCodeService
    {
        Task<IEnumerable<DiscountCodeDto>> GetAllAsync();
        Task<DiscountCodeDto> GetByIdAsync(int id);
        Task<DiscountCodeDto> GetByCodeAsync(string code);
        Task<IEnumerable<DiscountCodeDto>> GetActiveCodesAsync();
        Task<IEnumerable<DiscountCodeDto>> GetAvailableCodesAsync();
        Task<DiscountCodeDto> AddAsync(DiscountCodeDto discountCodeDto);
        Task<DiscountCodeDto> UpdateAsync(DiscountCodeDto discountCodeDto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(string code);
        Task<bool> IsCodeValidAsync(string code);
        Task<bool> IsCodeAvailableForUserAsync(string code, int userId);
        Task<decimal> CalculateDiscountAsync(string code, decimal originalAmount);
        Task<bool> UseCodeAsync(string code, int userId, int orderId);
    }
} 