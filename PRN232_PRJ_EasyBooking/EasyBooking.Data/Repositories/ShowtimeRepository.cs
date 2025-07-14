using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            return await _context.Showtimes.ToListAsync();
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
                _context.Showtimes.Remove(showtime);
                await _context.SaveChangesAsync();
            }
        }
    }
} 