using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EasyBooking.Data.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly CinemaBookingDbContext _context;
        public GenreRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres.Where(g => g.IsDelete != true).ToListAsync();
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task AddAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Genre genre)
        {
            _context.Genres.Update(genre);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var genre = await _context.Genres.FindAsync(id);
            if (genre != null)
            {
                // Soft delete: chỉ gán cờ, không xóa cứng
                genre.IsDelete = true;
                genre.DeleteAt = DateTime.Now;
                // DeleteBy, UpdateBy sẽ được truyền từ tầng trên nếu cần
                genre.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}