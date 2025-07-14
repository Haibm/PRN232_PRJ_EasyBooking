using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBooking.Business.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly ICinemaRepository _cinemaRepository;
        public CinemaService(ICinemaRepository cinemaRepository)
        {
            _cinemaRepository = cinemaRepository;
        }

        public async Task<IEnumerable<CinemaDto>> GetAllAsync()
        {
            var cinemas = await _cinemaRepository.GetAllAsync();
            return cinemas.Select(c => new CinemaDto
            {
                CinemaId = c.CinemaId,
                Name = c.Name,
                Address = c.Address,
                Phone = c.Phone
            });
        }

        public async Task<CinemaDto> GetByIdAsync(int id)
        {
            var c = await _cinemaRepository.GetByIdAsync(id);
            if (c == null) return null;
            return new CinemaDto
            {
                CinemaId = c.CinemaId,
                Name = c.Name,
                Address = c.Address,
                Phone = c.Phone
            };
        }

        public async Task AddAsync(CinemaDto cinemaDto)
        {
            var cinema = new Cinema
            {
                Name = cinemaDto.Name,
                Address = cinemaDto.Address,
                Phone = cinemaDto.Phone
            };
            await _cinemaRepository.AddAsync(cinema);
        }

        public async Task UpdateAsync(CinemaDto cinemaDto)
        {
            var cinema = new Cinema
            {
                CinemaId = cinemaDto.CinemaId,
                Name = cinemaDto.Name,
                Address = cinemaDto.Address,
                Phone = cinemaDto.Phone
            };
            await _cinemaRepository.UpdateAsync(cinema);
        }

        public async Task DeleteAsync(int id)
        {
            await _cinemaRepository.DeleteAsync(id);
        }
    }
} 