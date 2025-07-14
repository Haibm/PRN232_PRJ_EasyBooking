using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBooking.Business.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepository;
        public GenreService(IGenreRepository genreRepository)
        {
            _genreRepository = genreRepository;
        }

        public async Task<IEnumerable<GenreDto>> GetAllAsync()
        {
            var genres = await _genreRepository.GetAllAsync();
            return genres.Select(g => new GenreDto { GenreId = g.GenreId, Name = g.Name });
        }

        public async Task<GenreDto> GetByIdAsync(int id)
        {
            var g = await _genreRepository.GetByIdAsync(id);
            if (g == null) return null;
            return new GenreDto { GenreId = g.GenreId, Name = g.Name };
        }

        public async Task AddAsync(GenreDto genreDto)
        {
            var genre = new Genre { Name = genreDto.Name };
            await _genreRepository.AddAsync(genre);
        }

        public async Task UpdateAsync(GenreDto genreDto)
        {
            var genre = new Genre { GenreId = genreDto.GenreId, Name = genreDto.Name };
            await _genreRepository.UpdateAsync(genre);
        }

        public async Task DeleteAsync(int id)
        {
            await _genreRepository.DeleteAsync(id);
        }
    }
} 