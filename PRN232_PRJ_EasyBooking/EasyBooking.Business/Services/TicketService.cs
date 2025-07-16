using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace EasyBooking.Business.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            return tickets.Select(t => new TicketDto
            {
                TicketId = t.TicketId,
                ShowtimeId = t.ShowtimeId,
                UserId = t.UserId,
                SeatNumber = t.SeatNumber,
                BookingTime = t.BookingTime,
                Status = t.Status,
                PaymentMethod = t.PaymentMethod
            });
        }

        public async Task<TicketDto> GetByIdAsync(int id)
        {
            var t = await _ticketRepository.GetByIdAsync(id);
            if (t == null) return null;
            return new TicketDto
            {
                TicketId = t.TicketId,
                ShowtimeId = t.ShowtimeId,
                UserId = t.UserId,
                SeatNumber = t.SeatNumber,
                BookingTime = t.BookingTime,
                Status = t.Status,
                PaymentMethod = t.PaymentMethod
            };
        }

        public async Task AddAsync(TicketDto ticketDto)
        {
            var ticket = new Ticket
            {
                ShowtimeId = ticketDto.ShowtimeId,
                UserId = ticketDto.UserId,
                SeatNumber = ticketDto.SeatNumber,
                BookingTime = ticketDto.BookingTime,
                Status = ticketDto.Status,
                PaymentMethod = ticketDto.PaymentMethod
            };
            await _ticketRepository.AddAsync(ticket);
        }

        public async Task UpdateAsync(TicketDto ticketDto)
        {
            var ticket = new Ticket
            {
                TicketId = ticketDto.TicketId,
                ShowtimeId = ticketDto.ShowtimeId,
                UserId = ticketDto.UserId,
                SeatNumber = ticketDto.SeatNumber,
                BookingTime = ticketDto.BookingTime,
                Status = ticketDto.Status,
                PaymentMethod = ticketDto.PaymentMethod
            };
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task DeleteAsync(int id)
        {
            await _ticketRepository.DeleteAsync(id);
        }
    }
}