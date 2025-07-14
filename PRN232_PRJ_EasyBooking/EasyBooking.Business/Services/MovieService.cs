using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBooking.Business.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync()
        {
            var movies = await _movieRepository.GetAllAsync();
            return movies.Select(m => new MovieDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Description = m.Description,
                Duration = m.Duration,
                PosterUrl = m.PosterUrl,
                Status = m.Status
            });
        }

        public async Task<MovieDto> GetByIdAsync(int id)
        {
            var m = await _movieRepository.GetByIdAsync(id);
            if (m == null) return null;
            return new MovieDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Description = m.Description,
                Duration = m.Duration,
                PosterUrl = m.PosterUrl,
                Status = m.Status
            };
        }

        public async Task AddAsync(MovieDto movieDto)
        {
            var movie = new Movie
            {
                Title = movieDto.Title,
                Description = movieDto.Description,
                Duration = movieDto.Duration,
                PosterUrl = movieDto.PosterUrl,
                Status = movieDto.Status
            };
            await _movieRepository.AddAsync(movie);
        }

        public async Task UpdateAsync(MovieDto movieDto)
        {
            var movie = new Movie
            {
                MovieId = movieDto.MovieId,
                Title = movieDto.Title,
                Description = movieDto.Description,
                Duration = movieDto.Duration,
                PosterUrl = movieDto.PosterUrl,
                Status = movieDto.Status
            };
            await _movieRepository.UpdateAsync(movie);
        }

        public async Task DeleteAsync(int id)
        {
            await _movieRepository.DeleteAsync(id);
        }
    }
}
