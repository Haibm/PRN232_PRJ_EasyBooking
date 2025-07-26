using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EasyBooking.Data.Repositories
{
    public class CinemaRepository : ICinemaRepository
    {
        private readonly CinemaBookingDbContext _context;
        public CinemaRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cinema>> GetAllAsync()
        {
            return await _context.Cinemas.Where(c => c.IsDelete != true).ToListAsync();
        }

        public async Task<Cinema> GetByIdAsync(int id)
        {
            return await _context.Cinemas.FindAsync(id);
        }

        public async Task AddAsync(Cinema cinema)
        {
            await _context.Cinemas.AddAsync(cinema);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cinema cinema)
        {
            _context.Cinemas.Update(cinema);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var cinema = await _context.Cinemas.FindAsync(id);
            if (cinema != null)
            {
                // Soft delete: chỉ gán cờ, không xóa cứng
                cinema.IsDelete = true;
                cinema.DeleteAt = DateTime.Now;
                // DeleteBy, UpdateBy sẽ được truyền từ tầng trên nếu cần
                cinema.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}