using EasyBooking.Business.DTOs;

namespace EasyBooking.Business.Interfaces
{
    public interface IUserDiscountCodeService
    {
        Task<IEnumerable<UserDiscountCodeDto>> GetAllAsync();
        Task<UserDiscountCodeDto> GetByIdAsync(int id);
        Task<IEnumerable<UserDiscountCodeDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserDiscountCodeDto>> GetByDiscountCodeIdAsync(int discountCodeId);
        Task<UserDiscountCodeDto> GetByUserAndCodeAsync(int userId, int discountCodeId);
        Task<IEnumerable<UserDiscountCodeDto>> GetUnusedByUserIdAsync(int userId);
        Task<UserDiscountCodeDto> AddAsync(UserDiscountCodeDto userDiscountCodeDto);
        Task<UserDiscountCodeDto> UpdateAsync(UserDiscountCodeDto userDiscountCodeDto);
        Task DeleteAsync(int id);
        Task<bool> HasUserRedeemedCodeAsync(int userId, int discountCodeId);
        Task<int> GetUserRedeemedCountAsync(int userId, int discountCodeId);
        Task<UserDiscountCodeDto> RedeemCodeAsync(int userId, int discountCodeId, int pointsUsed, string createBy = "System");
        Task<bool> UseCodeAsync(int userDiscountCodeId, int orderId, string updateBy = "System");
    }
} 