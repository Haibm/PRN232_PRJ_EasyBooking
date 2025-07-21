using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBooking.Data.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly CinemaBookingDbContext _context;
        public SeatRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Seat>> GetAllAsync()
        {
            return await _context.Seats.ToListAsync();
        }
        public async Task<Seat> GetByIdAsync(int id)
        {
            return await _context.Seats.FindAsync(id);
        }
        public async Task<IEnumerable<Seat>> GetByRoomIdAsync(int roomId)
        {
            return await _context.Seats.Where(s => s.RoomId == roomId).ToListAsync();
        }
        public async Task AddAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat != null)
            {
                _context.Seats.Remove(seat);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ExistsAsync(int roomId, string rowLetter, int seatNumber)
        {
            return await _context.Seats.AnyAsync(s =>
                s.RoomId == roomId &&
                s.RowLetter == rowLetter &&
                s.SeatNumber == seatNumber
            );
        }
    }
} 