using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using EasyBooking.Data.DbContexts;

namespace EasyBooking.Data.Repositories
{
    public class PointTransactionRepository : IPointTransactionRepository
    {
        private readonly CinemaBookingDbContext _context;

        public PointTransactionRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PointTransaction>> GetAllAsync()
        {
            return await _context.PointTransactions
                .Where(pt => pt.IsDelete != true)
                .Include(pt => pt.User)
                .Include(pt => pt.Order)
                .Include(pt => pt.DiscountCode)
                .OrderByDescending(pt => pt.TransactionDate)
                .ToListAsync();
        }

        public async Task<PointTransaction> GetByIdAsync(int id)
        {
            return await _context.PointTransactions
                .Include(pt => pt.User)
                .Include(pt => pt.Order)
                .Include(pt => pt.DiscountCode)
                .FirstOrDefaultAsync(pt => pt.PointTransactionId == id && pt.IsDelete != true);
        }

        public async Task<IEnumerable<PointTransaction>> GetByUserIdAsync(int userId)
        {
            return await _context.PointTransactions
                .Where(pt => pt.UserId == userId && pt.IsDelete != true)
                .Include(pt => pt.Order)
                .Include(pt => pt.DiscountCode)
                .OrderByDescending(pt => pt.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PointTransaction>> GetByUserIdAndTypeAsync(int userId, string transactionType)
        {
            return await _context.PointTransactions
                .Where(pt => pt.UserId == userId && 
                            pt.TransactionType == transactionType && 
                            pt.IsDelete != true)
                .Include(pt => pt.Order)
                .Include(pt => pt.DiscountCode)
                .OrderByDescending(pt => pt.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PointTransaction>> GetByOrderIdAsync(int orderId)
        {
            return await _context.PointTransactions
                .Where(pt => pt.OrderId == orderId && pt.IsDelete != true)
                .Include(pt => pt.User)
                .Include(pt => pt.DiscountCode)
                .OrderByDescending(pt => pt.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<PointTransaction>> GetByDiscountCodeIdAsync(int discountCodeId)
        {
            return await _context.PointTransactions
                .Where(pt => pt.DiscountCodeId == discountCodeId && pt.IsDelete != true)
                .Include(pt => pt.User)
                .Include(pt => pt.Order)
                .OrderByDescending(pt => pt.TransactionDate)
                .ToListAsync();
        }

        public async Task<PointTransaction> AddAsync(PointTransaction pointTransaction)
        {
            _context.PointTransactions.Add(pointTransaction);
            await _context.SaveChangesAsync();
            return pointTransaction;
        }

        public async Task<PointTransaction> UpdateAsync(PointTransaction pointTransaction)
        {
            _context.PointTransactions.Update(pointTransaction);
            await _context.SaveChangesAsync();
            return pointTransaction;
        }

        public async Task DeleteAsync(int id)
        {
            var pointTransaction = await GetByIdAsync(id);
            if (pointTransaction != null)
            {
                pointTransaction.IsDelete = true;
                pointTransaction.DeleteAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUserTotalPointsAsync(int userId)
        {
            var earnedPoints = await _context.PointTransactions
                .Where(pt => pt.UserId == userId && 
                            pt.TransactionType == "EARN" && 
                            pt.IsDelete != true)
                .SumAsync(pt => pt.Points);

            var usedPoints = await _context.PointTransactions
                .Where(pt => pt.UserId == userId && 
                            (pt.TransactionType == "USE" || pt.TransactionType == "REDEEM") && 
                            pt.IsDelete != true)
                .SumAsync(pt => pt.Points);

            return earnedPoints - usedPoints;
        }

        public async Task<int> GetUserEarnedPointsAsync(int userId)
        {
            return await _context.PointTransactions
                .Where(pt => pt.UserId == userId && 
                            pt.TransactionType == "EARN" && 
                            pt.IsDelete != true)
                .SumAsync(pt => pt.Points);
        }

        public async Task<int> GetUserUsedPointsAsync(int userId)
        {
            return await _context.PointTransactions
                .Where(pt => pt.UserId == userId && 
                            (pt.TransactionType == "USE" || pt.TransactionType == "REDEEM") && 
                            pt.IsDelete != true)
                .SumAsync(pt => pt.Points);
        }
    }
} 