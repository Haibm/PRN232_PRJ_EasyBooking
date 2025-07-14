using EasyBooking.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Business.Interfaces
{
    public interface IRoomService
    {
        Task<IEnumerable<RoomDto>> GetAllAsync();
        Task<RoomDto> GetByIdAsync(int id);
        Task AddAsync(RoomDto roomDto);
        Task UpdateAsync(RoomDto roomDto);
        Task DeleteAsync(int id);
    }
} 