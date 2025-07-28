using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using EasyBooking.Data.DbContexts;

namespace EasyBooking.Data.Repositories
{
    public class DiscountCodeRepository : IDiscountCodeRepository
    {
        private readonly CinemaBookingDbContext _context;

        public DiscountCodeRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DiscountCode>> GetAllAsync()
        {
            return await _context.DiscountCodes
                .Where(dc => dc.IsDelete != true)
                .OrderByDescending(dc => dc.CreateAt)
                .ToListAsync();
        }

        public async Task<DiscountCode> GetByIdAsync(int id)
        {
            return await _context.DiscountCodes
                .FirstOrDefaultAsync(dc => dc.DiscountCodeId == id && dc.IsDelete != true);
        }

        public async Task<DiscountCode> GetByCodeAsync(string code)
        {
            return await _context.DiscountCodes
                .FirstOrDefaultAsync(dc => dc.Code == code && dc.IsDelete != true);
        }

        public async Task<IEnumerable<DiscountCode>> GetActiveCodesAsync()
        {
            return await _context.DiscountCodes
                .Where(dc => dc.IsActive == true && dc.IsDelete != true)
                .OrderByDescending(dc => dc.CreateAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<DiscountCode>> GetAvailableCodesAsync()
        {
            var now = DateTime.Now;
            return await _context.DiscountCodes
                .Where(dc => dc.IsActive == true && 
                            dc.IsDelete != true &&
                            dc.StartDate <= now &&
                            dc.EndDate >= now &&
                            (dc.MaxUsage == 0 || dc.CurrentUsage < dc.MaxUsage))
                .OrderBy(dc => dc.RequiredPoints)
                .ToListAsync();
        }

        public async Task<DiscountCode> AddAsync(DiscountCode discountCode)
        {
            _context.DiscountCodes.Add(discountCode);
            await _context.SaveChangesAsync();
            return discountCode;
        }

        public async Task<DiscountCode> UpdateAsync(DiscountCode discountCode)
        {
            _context.DiscountCodes.Update(discountCode);
            await _context.SaveChangesAsync();
            return discountCode;
        }

        public async Task DeleteAsync(int id)
        {
            var discountCode = await GetByIdAsync(id);
            if (discountCode != null)
            {
                discountCode.IsDelete = true;
                discountCode.DeleteAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(string code)
        {
            return await _context.DiscountCodes
                .AnyAsync(dc => dc.Code == code && dc.IsDelete != true);
        }

        public async Task<bool> IsCodeValidAsync(string code)
        {
            var now = DateTime.Now;
            return await _context.DiscountCodes
                .AnyAsync(dc => dc.Code == code &&
                               dc.IsActive == true &&
                               dc.IsDelete != true &&
                               dc.StartDate <= now &&
                               dc.EndDate >= now &&
                               (dc.MaxUsage == 0 || dc.CurrentUsage < dc.MaxUsage));
        }
    }
} 