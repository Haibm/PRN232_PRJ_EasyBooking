using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Data.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly CinemaBookingDbContext _context;
        public MovieRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }
        public async Task<Movie> GetDetailByIdAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Showtimes)
                    .ThenInclude(s => s.Room)
                        .ThenInclude(r => r.Cinema)
                .FirstOrDefaultAsync(m => m.MovieId == id);
        }
        public async Task AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
            }
        }
    }
} 