using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBooking.Business.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }
        public async Task<IEnumerable<SeatDto>> GetAllAsync()
        {
            var seats = await _seatRepository.GetAllAsync();
            return seats.Select(s => new SeatDto
            {
                SeatId = s.SeatId,
                RoomId = s.RoomId,
                RowLetter = s.RowLetter,
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType,
                IsDelete = s.IsDelete,
                CreateBy = s.CreateBy,
                CreateAt = s.CreateAt,
                UpdateBy = s.UpdateBy,
                UpdateAt = s.UpdateAt,
                DeleteBy = s.DeleteBy,
                DeleteAt = s.DeleteAt
            });
        }
        public async Task<SeatDto> GetByIdAsync(int id)
        {
            var s = await _seatRepository.GetByIdAsync(id);
            if (s == null) return null;
            return new SeatDto
            {
                SeatId = s.SeatId,
                RoomId = s.RoomId,
                RowLetter = s.RowLetter,
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType,
                IsDelete = s.IsDelete,
                CreateBy = s.CreateBy,
                CreateAt = s.CreateAt,
                UpdateBy = s.UpdateBy,
                UpdateAt = s.UpdateAt,
                DeleteBy = s.DeleteBy,
                DeleteAt = s.DeleteAt
            };
        }
        public async Task<IEnumerable<SeatDto>> GetByRoomIdAsync(int roomId)
        {
            var seats = await _seatRepository.GetByRoomIdAsync(roomId);
            return seats.Select(s => new SeatDto
            {
                SeatId = s.SeatId,
                RoomId = s.RoomId,
                RowLetter = s.RowLetter,
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType,
                IsDelete = s.IsDelete,
                CreateBy = s.CreateBy,
                CreateAt = s.CreateAt,
                UpdateBy = s.UpdateBy,
                UpdateAt = s.UpdateAt,
                DeleteBy = s.DeleteBy,
                DeleteAt = s.DeleteAt
            });
        }
        public async Task AddAsync(SeatDto seatDto)
        {
            // Kiểm tra ghế đã tồn tại chưa
            bool exists = await _seatRepository.ExistsAsync(seatDto.RoomId, seatDto.RowLetter, seatDto.SeatNumber);
            if (exists)
                throw new InvalidOperationException("Ghế đã tồn tại trong phòng này.");

            var seat = new Seat
            {
                RoomId = seatDto.RoomId,
                RowLetter = seatDto.RowLetter,
                SeatNumber = seatDto.SeatNumber,
                SeatType = seatDto.SeatType,
                IsDelete = seatDto.IsDelete,
                CreateBy = seatDto.CreateBy,
                CreateAt = seatDto.CreateAt,
                UpdateBy = seatDto.UpdateBy,
                UpdateAt = seatDto.UpdateAt,
                DeleteBy = seatDto.DeleteBy,
                DeleteAt = seatDto.DeleteAt
            };
            await _seatRepository.AddAsync(seat);
        }

        public async Task UpdateAsync(SeatDto seatDto)
        {
            var seat = new Seat
            {
                SeatId = seatDto.SeatId,
                RoomId = seatDto.RoomId,
                RowLetter = seatDto.RowLetter,
                SeatNumber = seatDto.SeatNumber,
                SeatType = seatDto.SeatType,
                IsDelete = seatDto.IsDelete,
                CreateBy = seatDto.CreateBy,
                CreateAt = seatDto.CreateAt,
                UpdateBy = seatDto.UpdateBy,
                UpdateAt = seatDto.UpdateAt,
                DeleteBy = seatDto.DeleteBy,
                DeleteAt = seatDto.DeleteAt
            };
            await _seatRepository.UpdateAsync(seat);
        }
        public async Task DeleteAsync(int id)
        {
            await _seatRepository.DeleteAsync(id);
        }

    }
}