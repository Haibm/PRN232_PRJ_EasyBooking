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
        private readonly IGenreRepository _genreRepository;
        public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
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
            // Lấy danh sách genre từ DB theo tên
            var allGenres = await _genreRepository.GetAllAsync();
            var selectedGenres = allGenres.Where(g => movieDto.Genres.Contains(g.Name)).ToList();
            movie.Genres = selectedGenres;
            await _movieRepository.AddAsync(movie);
        }

        public async Task UpdateAsync(MovieDto movieDto)
        {
            var movie = await _movieRepository.GetByIdAsync(movieDto.MovieId);
            if (movie == null) return;
            
            // Cập nhật các thuộc tính cơ bản
            movie.Title = movieDto.Title;
            movie.Description = movieDto.Description;
            movie.Duration = movieDto.Duration;
            movie.PosterUrl = movieDto.PosterUrl;
            movie.Status = movieDto.Status;
            
            // Chuẩn bị genres để cập nhật
            var allGenres = await _genreRepository.GetAllAsync();
            var selectedGenres = allGenres.Where(g => movieDto.Genres.Contains(g.Name)).ToList();
            movie.Genres = selectedGenres;
            
            await _movieRepository.UpdateAsync(movie);
        }

        public async Task DeleteAsync(int id)
        {
            await _movieRepository.DeleteAsync(id);
        }
    }
}
