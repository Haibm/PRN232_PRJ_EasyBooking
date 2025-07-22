using EasyBooking.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Data.Repositories
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetAllAsync();
        Task<Seat> GetByIdAsync(int id);
        Task<IEnumerable<Seat>> GetByRoomIdAsync(int roomId);
        Task AddAsync(Seat seat);
        Task UpdateAsync(Seat seat);
        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int roomId, string rowLetter, int seatNumber);
    }
} 