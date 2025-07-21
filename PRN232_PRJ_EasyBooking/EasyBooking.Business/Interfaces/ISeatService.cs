using EasyBooking.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Business.Interfaces
{
    public interface ISeatService
    {
        Task<IEnumerable<SeatDto>> GetAllAsync();
        Task<SeatDto> GetByIdAsync(int id);
        Task<IEnumerable<SeatDto>> GetByRoomIdAsync(int roomId);
        Task AddAsync(SeatDto seatDto);
        Task UpdateAsync(SeatDto seatDto);
        Task DeleteAsync(int id);
    }
} 