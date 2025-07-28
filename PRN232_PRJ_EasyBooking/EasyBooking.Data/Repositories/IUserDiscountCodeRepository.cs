using EasyBooking.Data.Entities;

namespace EasyBooking.Data.Repositories
{
    public interface IUserDiscountCodeRepository
    {
        Task<IEnumerable<UserDiscountCode>> GetAllAsync();
        Task<UserDiscountCode> GetByIdAsync(int id);
        Task<IEnumerable<UserDiscountCode>> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserDiscountCode>> GetByDiscountCodeIdAsync(int discountCodeId);
        Task<UserDiscountCode> GetByUserAndCodeAsync(int userId, int discountCodeId);
        Task<IEnumerable<UserDiscountCode>> GetUnusedByUserIdAsync(int userId);
        Task<UserDiscountCode> AddAsync(UserDiscountCode userDiscountCode);
        Task<UserDiscountCode> UpdateAsync(UserDiscountCode userDiscountCode);
        Task DeleteAsync(int id);
        Task<bool> HasUserRedeemedCodeAsync(int userId, int discountCodeId);
        Task<int> GetUserRedeemedCountAsync(int userId, int discountCodeId);
    }
} 