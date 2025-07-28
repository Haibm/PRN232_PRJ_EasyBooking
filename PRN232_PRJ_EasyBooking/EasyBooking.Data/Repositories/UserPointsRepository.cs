using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using EasyBooking.Data.DbContexts;

namespace EasyBooking.Data.Repositories
{
    public class UserPointsRepository : IUserPointsRepository
    {
        private readonly CinemaBookingDbContext _context;

        public UserPointsRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserPoints>> GetAllAsync()
        {
            return await _context.UserPoints
                .Where(up => up.IsDelete != true)
                .Include(up => up.User)
                .ToListAsync();
        }

        public async Task<UserPoints> GetByIdAsync(int id)
        {
            return await _context.UserPoints
                .Include(up => up.User)
                .FirstOrDefaultAsync(up => up.UserPointsId == id && up.IsDelete != true);
        }

        public async Task<UserPoints> GetByUserIdAsync(int userId)
        {
            return await _context.UserPoints
                .Include(up => up.User)
                .FirstOrDefaultAsync(up => up.UserId == userId && up.IsDelete != true);
        }

        public async Task<UserPoints> AddAsync(UserPoints userPoints)
        {
            _context.UserPoints.Add(userPoints);
            await _context.SaveChangesAsync();
            return userPoints;
        }

        public async Task<UserPoints> UpdateAsync(UserPoints userPoints)
        {
            _context.UserPoints.Update(userPoints);
            await _context.SaveChangesAsync();
            return userPoints;
        }

        public async Task DeleteAsync(int id)
        {
            var userPoints = await GetByIdAsync(id);
            if (userPoints != null)
            {
                userPoints.IsDelete = true;
                userPoints.DeleteAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int userId)
        {
            return await _context.UserPoints
                .AnyAsync(up => up.UserId == userId && up.IsDelete != true);
        }
    }
} 