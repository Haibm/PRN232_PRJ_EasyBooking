using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

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
                Status = m.Status,
                Genres = m.Genres?.Select(g => g.Name).ToList() ?? new List<string>(),
                Showtimes = m.Showtimes?.Select(s => s.StartTime).ToList() ?? new List<DateTime>()
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
                Status = m.Status,
                Genres = m.Genres?.Select(g => g.Name).ToList() ?? new List<string>(),
                Showtimes = m.Showtimes?.Select(s => s.StartTime).ToList() ?? new List<DateTime>()
            };
        }
        public async Task<MovieDetailDto> GetDetailByIdAsync(int id)
        {
            var m = await _movieRepository.GetDetailByIdAsync(id);
            if (m == null) return null;
            return new MovieDetailDto
            {
                MovieId = m.MovieId,
                Title = m.Title,
                Description = m.Description,
                Duration = m.Duration,
                PosterUrl = m.PosterUrl,
                Status = m.Status,
                Genres = m.Genres?.Select(g => g.Name).ToList() ?? new List<string>(),
                Showtimes = m.Showtimes?.Select(s => new ShowtimeDetailDto
                {
                    ShowtimeId = s.ShowtimeId,
                    StartTime = s.StartTime,
                    Price = s.Price,
                    RoomName = s.Room?.Name ?? string.Empty,
                    CinemaName = s.Room?.Cinema?.Name ?? string.Empty
                }).ToList() ?? new List<ShowtimeDetailDto>()
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
