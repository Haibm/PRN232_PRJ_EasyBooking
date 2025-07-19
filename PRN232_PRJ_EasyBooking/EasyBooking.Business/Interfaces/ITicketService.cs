using EasyBooking.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Business.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task<TicketDto> GetByIdAsync(int id);
        Task AddAsync(TicketDto ticketDto);
        Task UpdateAsync(TicketDto ticketDto);
        Task DeleteAsync(int id);
    }
}