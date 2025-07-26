using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EasyBooking.Data.Repositories
{
    public class RefundHistoryRepository : IRefundHistoryRepository
    {
        private readonly CinemaBookingDbContext _context;
        public RefundHistoryRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RefundHistory>> GetAllAsync()
        {
            return await _context.RefundHistories
                .Include(r => r.Ticket)
                .Include(r => r.User)
                .ToListAsync();
        }

        public async Task<RefundHistory> GetByIdAsync(int id)
        {
            return await _context.RefundHistories
                .Include(r => r.Ticket)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.RefundHistoryId == id);
        }

        public async Task<IEnumerable<RefundHistory>> GetByUserIdAsync(int userId)
        {
            return await _context.RefundHistories
                .Include(r => r.Ticket)
                .Include(r => r.User)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<RefundHistory>> GetByTicketIdAsync(int ticketId)
        {
            return await _context.RefundHistories
                .Include(r => r.Ticket)
                .Include(r => r.User)
                .Where(r => r.TicketId == ticketId)
                .ToListAsync();
        }

        public async Task AddAsync(RefundHistory refundHistory)
        {
            await _context.RefundHistories.AddAsync(refundHistory);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefundHistory refundHistory)
        {
            _context.RefundHistories.Update(refundHistory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var refundHistory = await _context.RefundHistories.FindAsync(id);
            if (refundHistory != null)
            {
                _context.RefundHistories.Remove(refundHistory);
                await _context.SaveChangesAsync();
            }
        }
    }
} 