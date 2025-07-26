using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBooking.Business.Services
{
    public class RefundHistoryService : IRefundHistoryService
    {
        private readonly IRefundHistoryRepository _refundHistoryRepository;
        public RefundHistoryService(IRefundHistoryRepository refundHistoryRepository)
        {
            _refundHistoryRepository = refundHistoryRepository;
        }

        public async Task<IEnumerable<RefundHistoryDto>> GetAllAsync()
        {
            var refundHistories = await _refundHistoryRepository.GetAllAsync();
            return refundHistories.Select(r => new RefundHistoryDto
            {
                RefundHistoryId = r.RefundHistoryId,
                TicketId = r.TicketId,
                RefundAmount = r.RefundAmount,
                RefundTime = r.RefundTime,
                Reason = r.Reason,
                UserId = r.UserId,
                CreateBy = r.CreateBy,
                CreateAt = r.CreateAt,
                MovieTitle = r.Ticket?.Showtime?.Movie?.Title,
                CinemaName = r.Ticket?.Showtime?.Room?.Cinema?.Name,
                RoomName = r.Ticket?.Showtime?.Room?.Name,
                SeatNumber = r.Ticket?.SeatNumber,
                ShowtimeStart = r.Ticket?.Showtime?.StartTime,
                UserName = r.User?.FullName
            });
        }

        public async Task<RefundHistoryDto> GetByIdAsync(int id)
        {
            var r = await _refundHistoryRepository.GetByIdAsync(id);
            if (r == null) return null;
            return new RefundHistoryDto
            {
                RefundHistoryId = r.RefundHistoryId,
                TicketId = r.TicketId,
                RefundAmount = r.RefundAmount,
                RefundTime = r.RefundTime,
                Reason = r.Reason,
                UserId = r.UserId,
                CreateBy = r.CreateBy,
                CreateAt = r.CreateAt,
                MovieTitle = r.Ticket?.Showtime?.Movie?.Title,
                CinemaName = r.Ticket?.Showtime?.Room?.Cinema?.Name,
                RoomName = r.Ticket?.Showtime?.Room?.Name,
                SeatNumber = r.Ticket?.SeatNumber,
                ShowtimeStart = r.Ticket?.Showtime?.StartTime,
                UserName = r.User?.FullName
            };
        }

        public async Task<IEnumerable<RefundHistoryDto>> GetByUserIdAsync(int userId)
        {
            var refundHistories = await _refundHistoryRepository.GetByUserIdAsync(userId);
            return refundHistories.Select(r => new RefundHistoryDto
            {
                RefundHistoryId = r.RefundHistoryId,
                TicketId = r.TicketId,
                RefundAmount = r.RefundAmount,
                RefundTime = r.RefundTime,
                Reason = r.Reason,
                UserId = r.UserId,
                CreateBy = r.CreateBy,
                CreateAt = r.CreateAt,
                MovieTitle = r.Ticket?.Showtime?.Movie?.Title,
                CinemaName = r.Ticket?.Showtime?.Room?.Cinema?.Name,
                RoomName = r.Ticket?.Showtime?.Room?.Name,
                SeatNumber = r.Ticket?.SeatNumber,
                ShowtimeStart = r.Ticket?.Showtime?.StartTime,
                UserName = r.User?.FullName
            });
        }

        public async Task<IEnumerable<RefundHistoryDto>> GetByTicketIdAsync(int ticketId)
        {
            var refundHistories = await _refundHistoryRepository.GetByTicketIdAsync(ticketId);
            return refundHistories.Select(r => new RefundHistoryDto
            {
                RefundHistoryId = r.RefundHistoryId,
                TicketId = r.TicketId,
                RefundAmount = r.RefundAmount,
                RefundTime = r.RefundTime,
                Reason = r.Reason,
                UserId = r.UserId,
                CreateBy = r.CreateBy,
                CreateAt = r.CreateAt,
                MovieTitle = r.Ticket?.Showtime?.Movie?.Title,
                CinemaName = r.Ticket?.Showtime?.Room?.Cinema?.Name,
                RoomName = r.Ticket?.Showtime?.Room?.Name,
                SeatNumber = r.Ticket?.SeatNumber,
                ShowtimeStart = r.Ticket?.Showtime?.StartTime,
                UserName = r.User?.FullName
            });
        }

        public async Task AddAsync(RefundHistoryDto refundHistoryDto)
        {
            var refundHistory = new RefundHistory
            {
                TicketId = refundHistoryDto.TicketId,
                RefundAmount = refundHistoryDto.RefundAmount,
                RefundTime = refundHistoryDto.RefundTime,
                Reason = refundHistoryDto.Reason,
                UserId = refundHistoryDto.UserId,
                CreateBy = refundHistoryDto.CreateBy,
                CreateAt = refundHistoryDto.CreateAt
            };
            await _refundHistoryRepository.AddAsync(refundHistory);
        }

        public async Task UpdateAsync(RefundHistoryDto refundHistoryDto)
        {
            var refundHistory = new RefundHistory
            {
                RefundHistoryId = refundHistoryDto.RefundHistoryId,
                TicketId = refundHistoryDto.TicketId,
                RefundAmount = refundHistoryDto.RefundAmount,
                RefundTime = refundHistoryDto.RefundTime,
                Reason = refundHistoryDto.Reason,
                UserId = refundHistoryDto.UserId,
                CreateBy = refundHistoryDto.CreateBy,
                CreateAt = refundHistoryDto.CreateAt
            };
            await _refundHistoryRepository.UpdateAsync(refundHistory);
        }

        public async Task DeleteAsync(int id)
        {
            await _refundHistoryRepository.DeleteAsync(id);
        }
    }
} 