using EasyBooking.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Business.Interfaces
{
    public interface ICinemaService
    {
        Task<IEnumerable<CinemaDto>> GetAllAsync();
        Task<CinemaDto> GetByIdAsync(int id);
        Task AddAsync(CinemaDto cinemaDto);
        Task UpdateAsync(CinemaDto cinemaDto);
        Task DeleteAsync(int id);
    }
} 