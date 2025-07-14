using EasyBooking.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Business.Interfaces
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDto>> GetAllAsync();
        Task<GenreDto> GetByIdAsync(int id);
        Task AddAsync(GenreDto genreDto);
        Task UpdateAsync(GenreDto genreDto);
        Task DeleteAsync(int id);
    }
} 