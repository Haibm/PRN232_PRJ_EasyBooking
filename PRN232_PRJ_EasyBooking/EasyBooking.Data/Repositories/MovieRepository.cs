using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
            return await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Showtimes)
                .ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Showtimes)
                .FirstOrDefaultAsync(m => m.MovieId == id);
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
            // Cập nhật movie cơ bản bằng raw SQL
            await _context.Database.ExecuteSqlRawAsync(
                "UPDATE Movies SET Title = {0}, Description = {1}, Duration = {2}, PosterUrl = {3}, Status = {4} WHERE MovieId = {5}",
                movie.Title, movie.Description, movie.Duration, movie.PosterUrl, movie.Status, movie.MovieId);

            // Xóa tất cả genres cũ
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM MovieGenres WHERE MovieId = {0}",
                movie.MovieId);

            // Thêm genres mới
            foreach (var genre in movie.Genres)
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "INSERT INTO MovieGenres (MovieId, GenreId) VALUES ({0}, {1})",
                    movie.MovieId, genre.GenreId);
            }
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