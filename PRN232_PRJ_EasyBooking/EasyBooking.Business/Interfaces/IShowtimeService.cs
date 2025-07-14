using EasyBooking.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Business.Interfaces
{
    public interface IShowtimeService
    {
        Task<IEnumerable<ShowtimeDto>> GetAllAsync();
        Task<ShowtimeDto> GetByIdAsync(int id);
        Task AddAsync(ShowtimeDto showtimeDto);
        Task UpdateAsync(ShowtimeDto showtimeDto);
        Task DeleteAsync(int id);
    }
} 