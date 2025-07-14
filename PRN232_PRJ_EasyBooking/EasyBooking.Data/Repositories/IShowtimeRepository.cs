using EasyBooking.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Data.Repositories
{
    public interface IShowtimeRepository
    {
        Task<IEnumerable<Showtime>> GetAllAsync();
        Task<Showtime> GetByIdAsync(int id);
        Task AddAsync(Showtime showtime);
        Task UpdateAsync(Showtime showtime);
        Task DeleteAsync(int id);
    }
} 