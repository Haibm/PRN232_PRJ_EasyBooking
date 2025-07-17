using EasyBooking.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Business.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllAsync();
        Task<MovieDto> GetByIdAsync(int id);
        Task<MovieDetailDto> GetDetailByIdAsync(int id);
        Task AddAsync(MovieDto movieDto);
        Task UpdateAsync(MovieDto movieDto);
        Task DeleteAsync(int id);
    }
}
