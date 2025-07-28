using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using EasyBooking.Data.DbContexts;

namespace EasyBooking.Data.Repositories
{
    public class PointConfigRepository : IPointConfigRepository
    {
        private readonly CinemaBookingDbContext _context;

        public PointConfigRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PointConfig>> GetAllAsync()
        {
            Console.WriteLine("PointConfigRepository.GetAllAsync called");
            try
            {
                // Kiểm tra context có null không
                if (_context == null)
                {
                    Console.WriteLine("PointConfigRepository.GetAllAsync: _context is null");
                    throw new InvalidOperationException("Database context is null");
                }

                // Kiểm tra PointConfigs DbSet có null không
                if (_context.PointConfigs == null)
                {
                    Console.WriteLine("PointConfigRepository.GetAllAsync: _context.PointConfigs is null");
                    throw new InvalidOperationException("PointConfigs DbSet is null");
                }

                Console.WriteLine("PointConfigRepository.GetAllAsync: About to query database");
                
                // Thử query đơn giản trước
                var allConfigs = await _context.PointConfigs.ToListAsync();
                Console.WriteLine($"PointConfigRepository.GetAllAsync: Raw query returned {allConfigs.Count} entities");
                
                // Debug: Log tất cả entities
                foreach (var entity in allConfigs)
                {
                    Console.WriteLine($"Raw Entity: ID={entity.PointConfigId}, Name={entity.ConfigName}, IsDelete={entity.IsDelete}, IsActive={entity.IsActive}");
                }
                
                // Sau đó filter theo điều kiện
                var result = allConfigs
                    .Where(pc => pc.IsDelete != true)
                    .OrderByDescending(pc => pc.CreateAt)
                    .ToList();
                
                Console.WriteLine($"PointConfigRepository.GetAllAsync returned {result.Count} filtered entities");
                
                // Debug: Log từng entity đã filter
                foreach (var entity in result)
                {
                    Console.WriteLine($"Filtered Entity: ID={entity.PointConfigId}, Name={entity.ConfigName}, IsDelete={entity.IsDelete}");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PointConfigRepository.GetAllAsync error: {ex.Message}");
                Console.WriteLine($"PointConfigRepository.GetAllAsync stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<PointConfig> GetByIdAsync(int id)
        {
            return await _context.PointConfigs
                .FirstOrDefaultAsync(pc => pc.PointConfigId == id && pc.IsDelete != true);
        }

        public async Task<PointConfig> GetActiveConfigAsync()
        {
            return await _context.PointConfigs
                .FirstOrDefaultAsync(pc => pc.IsActive && pc.IsDelete != true);
        }

        public async Task<PointConfig> AddAsync(PointConfig pointConfig)
        {
            _context.PointConfigs.Add(pointConfig);
            await _context.SaveChangesAsync();
            return pointConfig;
        }

        public async Task<PointConfig> UpdateAsync(PointConfig pointConfig)
        {
            _context.PointConfigs.Update(pointConfig);
            await _context.SaveChangesAsync();
            return pointConfig;
        }

        public async Task DeleteAsync(int id)
        {
            var pointConfig = await GetByIdAsync(id);
            if (pointConfig != null)
            {
                pointConfig.IsDelete = true;
                pointConfig.DeleteAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.PointConfigs
                .AnyAsync(pc => pc.PointConfigId == id && pc.IsDelete != true);
        }
    }
} 