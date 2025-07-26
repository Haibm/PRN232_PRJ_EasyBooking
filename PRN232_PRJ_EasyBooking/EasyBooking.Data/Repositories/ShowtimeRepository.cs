using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EasyBooking.Data.Repositories
{
    public class ShowtimeRepository : IShowtimeRepository
    {
        private readonly CinemaBookingDbContext _context;
        public ShowtimeRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Showtime>> GetAllAsync()
        {
            return await _context.Showtimes.Where(s => s.IsDelete != true).ToListAsync();
        }

        public async Task<Showtime> GetByIdAsync(int id)
        {
            return await _context.Showtimes.FindAsync(id);
        }

        public async Task AddAsync(Showtime showtime)
        {
            await _context.Showtimes.AddAsync(showtime);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Showtime showtime)
        {
            _context.Showtimes.Update(showtime);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var showtime = await _context.Showtimes.FindAsync(id);
            if (showtime != null)
            {
                // Soft delete: chỉ gán cờ, không xóa cứng
                showtime.IsDelete = true;
                showtime.DeleteAt = DateTime.Now;
                // DeleteBy, UpdateBy sẽ được truyền từ tầng trên nếu cần
                showtime.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}