using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBooking.Business.Services
{
    public class ShowtimeService : IShowtimeService
    {
        private readonly IShowtimeRepository _showtimeRepository;
        public ShowtimeService(IShowtimeRepository showtimeRepository)
        {
            _showtimeRepository = showtimeRepository;
        }

        public async Task<IEnumerable<ShowtimeDto>> GetAllAsync()
        {
            var showtimes = await _showtimeRepository.GetAllAsync();
            return showtimes.Select(s => new ShowtimeDto
            {
                ShowtimeId = s.ShowtimeId,
                MovieId = s.MovieId,
                RoomId = s.RoomId,
                StartTime = s.StartTime,
                Price = s.Price
            });
        }

        public async Task<ShowtimeDto> GetByIdAsync(int id)
        {
            var s = await _showtimeRepository.GetByIdAsync(id);
            if (s == null) return null;
            return new ShowtimeDto
            {
                ShowtimeId = s.ShowtimeId,
                MovieId = s.MovieId,
                RoomId = s.RoomId,
                StartTime = s.StartTime,
                Price = s.Price
            };
        }

        public async Task AddAsync(ShowtimeDto showtimeDto)
        {
            var showtime = new Showtime
            {
                MovieId = showtimeDto.MovieId,
                RoomId = showtimeDto.RoomId,
                StartTime = showtimeDto.StartTime,
                Price = showtimeDto.Price
            };
            await _showtimeRepository.AddAsync(showtime);
        }

        public async Task UpdateAsync(ShowtimeDto showtimeDto)
        {
            var showtime = new Showtime
            {
                ShowtimeId = showtimeDto.ShowtimeId,
                MovieId = showtimeDto.MovieId,
                RoomId = showtimeDto.RoomId,
                StartTime = showtimeDto.StartTime,
                Price = showtimeDto.Price
            };
            await _showtimeRepository.UpdateAsync(showtime);
        }

        public async Task DeleteAsync(int id)
        {
            await _showtimeRepository.DeleteAsync(id);
        }
    }
} 