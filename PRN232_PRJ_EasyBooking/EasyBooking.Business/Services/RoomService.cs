using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBooking.Business.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return rooms.Select(r => new RoomDto
            {
                RoomId = r.RoomId,
                Name = r.Name,
                Capacity = r.Capacity,
                Location = r.Location,
                CinemaId = r.CinemaId
            });
        }

        public async Task<RoomDto> GetByIdAsync(int id)
        {
            var r = await _roomRepository.GetByIdAsync(id);
            if (r == null) return null;
            return new RoomDto
            {
                RoomId = r.RoomId,
                Name = r.Name,
                Capacity = r.Capacity,
                Location = r.Location,
                CinemaId = r.CinemaId
            };
        }

        public async Task AddAsync(RoomDto roomDto)
        {
            var room = new Room
            {
                Name = roomDto.Name,
                Capacity = roomDto.Capacity,
                Location = roomDto.Location,
                CinemaId = roomDto.CinemaId
            };
            await _roomRepository.AddAsync(room);
        }

        public async Task UpdateAsync(RoomDto roomDto)
        {
            var room = new Room
            {
                RoomId = roomDto.RoomId,
                Name = roomDto.Name,
                Capacity = roomDto.Capacity,
                Location = roomDto.Location,
                CinemaId = roomDto.CinemaId
            };
            await _roomRepository.UpdateAsync(room);
        }

        public async Task DeleteAsync(int id)
        {
            await _roomRepository.DeleteAsync(id);
        }
    }
} 