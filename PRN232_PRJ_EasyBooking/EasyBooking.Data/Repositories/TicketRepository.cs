using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EasyBooking.Data.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly CinemaBookingDbContext _context;
        public TicketRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.Where(t => t.IsDelete != true).ToListAsync();
        }

        public async Task<Ticket> GetByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public void Add(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                // Soft delete: chỉ gán cờ, không xóa cứng
                ticket.IsDelete = true;
                ticket.DeleteAt = DateTime.Now;
                // DeleteBy, UpdateBy sẽ được truyền từ tầng trên nếu cần
                ticket.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
        public IEnumerable<Ticket> GetByOrderHistoryIdWithDetails(int orderHistoryId)
        {
            return _context.Tickets
                .Where(t => t.OrderHistoryId == orderHistoryId && t.IsDelete != true)
                .Include(t => t.Showtime)
                    .ThenInclude(s => s.Room)
                        .ThenInclude(r => r.Cinema)
                .Include(t => t.Showtime)
                    .ThenInclude(s => s.Movie)
                .ToList();
        }

        public IEnumerable<Ticket> GetAllByOrderHistoryIdWithDetails(int orderHistoryId)
        {
            return _context.Tickets
                .Where(t => t.OrderHistoryId == orderHistoryId)
                .Include(t => t.Showtime)
                    .ThenInclude(s => s.Room)
                        .ThenInclude(r => r.Cinema)
                .Include(t => t.Showtime)
                    .ThenInclude(s => s.Movie)
                .ToList();
        }

        public async Task<IEnumerable<Ticket>> GetByShowtimeIdAsync(int showtimeId)
        {
            return await _context.Tickets.Where(t => t.ShowtimeId == showtimeId && t.IsDelete != true).ToListAsync();
        }
    }
}