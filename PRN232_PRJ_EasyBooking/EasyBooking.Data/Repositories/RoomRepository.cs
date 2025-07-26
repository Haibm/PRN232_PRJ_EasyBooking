using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace EasyBooking.Data.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly CinemaBookingDbContext _context;
        public RoomRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms.Where(r => r.IsDelete != true).ToListAsync();
        }

        public async Task<Room> GetByIdAsync(int id)
        {
            return await _context.Rooms.FindAsync(id);
        }

        public async Task AddAsync(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                // Soft delete: chỉ gán cờ, không xóa cứng
                room.IsDelete = true;
                room.DeleteAt = DateTime.Now;
                // DeleteBy, UpdateBy sẽ được truyền từ tầng trên nếu cần
                room.UpdateAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}