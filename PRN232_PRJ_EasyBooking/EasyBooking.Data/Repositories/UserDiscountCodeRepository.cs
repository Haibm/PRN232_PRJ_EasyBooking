using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using EasyBooking.Data.DbContexts;

namespace EasyBooking.Data.Repositories
{
    public class UserDiscountCodeRepository : IUserDiscountCodeRepository
    {
        private readonly CinemaBookingDbContext _context;

        public UserDiscountCodeRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDiscountCode>> GetAllAsync()
        {
            return await _context.UserDiscountCodes
                .Where(udc => udc.IsDelete != true)
                .Include(udc => udc.User)
                .Include(udc => udc.DiscountCode)
                .Include(udc => udc.Order)
                .OrderByDescending(udc => udc.RedeemDate)
                .ToListAsync();
        }

        public async Task<UserDiscountCode> GetByIdAsync(int id)
        {
            return await _context.UserDiscountCodes
                .Include(udc => udc.User)
                .Include(udc => udc.DiscountCode)
                .Include(udc => udc.Order)
                .FirstOrDefaultAsync(udc => udc.UserDiscountCodeId == id && udc.IsDelete != true);
        }

        public async Task<IEnumerable<UserDiscountCode>> GetByUserIdAsync(int userId)
        {
            return await _context.UserDiscountCodes
                .Where(udc => udc.UserId == userId && udc.IsDelete != true)
                .Include(udc => udc.DiscountCode)
                .Include(udc => udc.Order)
                .OrderByDescending(udc => udc.RedeemDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDiscountCode>> GetByDiscountCodeIdAsync(int discountCodeId)
        {
            return await _context.UserDiscountCodes
                .Where(udc => udc.DiscountCodeId == discountCodeId && udc.IsDelete != true)
                .Include(udc => udc.User)
                .Include(udc => udc.Order)
                .OrderByDescending(udc => udc.RedeemDate)
                .ToListAsync();
        }

        public async Task<UserDiscountCode> GetByUserAndCodeAsync(int userId, int discountCodeId)
        {
            return await _context.UserDiscountCodes
                .Include(udc => udc.DiscountCode)
                .Include(udc => udc.Order)
                .FirstOrDefaultAsync(udc => udc.UserId == userId && 
                                          udc.DiscountCodeId == discountCodeId && 
                                          udc.IsDelete != true);
        }

        public async Task<IEnumerable<UserDiscountCode>> GetUnusedByUserIdAsync(int userId)
        {
            return await _context.UserDiscountCodes
                .Where(udc => udc.UserId == userId && 
                             udc.IsUsed == false && 
                             udc.IsDelete != true)
                .Include(udc => udc.DiscountCode)
                .OrderByDescending(udc => udc.RedeemDate)
                .ToListAsync();
        }

        public async Task<UserDiscountCode> AddAsync(UserDiscountCode userDiscountCode)
        {
            _context.UserDiscountCodes.Add(userDiscountCode);
            await _context.SaveChangesAsync();
            return userDiscountCode;
        }

        public async Task<UserDiscountCode> UpdateAsync(UserDiscountCode userDiscountCode)
        {
            _context.UserDiscountCodes.Update(userDiscountCode);
            await _context.SaveChangesAsync();
            return userDiscountCode;
        }

        public async Task DeleteAsync(int id)
        {
            var userDiscountCode = await GetByIdAsync(id);
            if (userDiscountCode != null)
            {
                userDiscountCode.IsDelete = true;
                userDiscountCode.DeleteAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasUserRedeemedCodeAsync(int userId, int discountCodeId)
        {
            return await _context.UserDiscountCodes
                .AnyAsync(udc => udc.UserId == userId && 
                                udc.DiscountCodeId == discountCodeId && 
                                udc.IsDelete != true);
        }

        public async Task<int> GetUserRedeemedCountAsync(int userId, int discountCodeId)
        {
            return await _context.UserDiscountCodes
                .CountAsync(udc => udc.UserId == userId && 
                                  udc.DiscountCodeId == discountCodeId && 
                                  udc.IsDelete != true);
        }
    }
} 