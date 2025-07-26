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
        private readonly IRefundHistoryService _refundHistoryService;
        private readonly IRefundPolicyService _refundPolicyService;
        public TicketService(ITicketRepository ticketRepository, IRefundHistoryService refundHistoryService, IRefundPolicyService refundPolicyService)
        {
            _ticketRepository = ticketRepository;
            _refundHistoryService = refundHistoryService;
            _refundPolicyService = refundPolicyService;
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
                OrderHistoryId = t.OrderHistoryId,
                IsDelete = t.IsDelete,
                CreateBy = t.CreateBy,
                CreateAt = t.CreateAt,
                UpdateBy = t.UpdateBy,
                UpdateAt = t.UpdateAt,
                DeleteBy = t.DeleteBy,
                DeleteAt = t.DeleteAt
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
                OrderHistoryId = t.OrderHistoryId,
                IsDelete = t.IsDelete,
                CreateBy = t.CreateBy,
                CreateAt = t.CreateAt,
                UpdateBy = t.UpdateBy,
                UpdateAt = t.UpdateAt,
                DeleteBy = t.DeleteBy,
                DeleteAt = t.DeleteAt
            };
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
                OrderHistoryId = ticketDto.OrderHistoryId,
                IsDelete = ticketDto.IsDelete,
                CreateBy = ticketDto.CreateBy,
                CreateAt = ticketDto.CreateAt,
                UpdateBy = ticketDto.UpdateBy,
                UpdateAt = ticketDto.UpdateAt,
                DeleteBy = ticketDto.DeleteBy,
                DeleteAt = ticketDto.DeleteAt
            };
            await _ticketRepository.UpdateAsync(ticket);
        }

        public async Task DeleteAsync(int id)
        {
            await _ticketRepository.DeleteAsync(id);
        }

        public TicketDto CreateTicket(TicketDto ticketDto)
        {
            var ticket = new Ticket
            {
                UserId = ticketDto.UserId,
                ShowtimeId = ticketDto.ShowtimeId,
                SeatNumber = ticketDto.SeatNumber,
                BookingTime = ticketDto.BookingTime,
                Status = ticketDto.Status,
                OrderHistoryId = ticketDto.OrderHistoryId,
                IsDelete = ticketDto.IsDelete,
                CreateBy = ticketDto.CreateBy,
                CreateAt = ticketDto.CreateAt,
                UpdateBy = ticketDto.UpdateBy,
                UpdateAt = ticketDto.UpdateAt,
                DeleteBy = ticketDto.DeleteBy,
                DeleteAt = ticketDto.DeleteAt
            };
            _ticketRepository.Add(ticket);
            return new TicketDto
            {
                TicketId = ticket.TicketId,
                UserId = ticket.UserId,
                ShowtimeId = ticket.ShowtimeId,
                SeatNumber = ticket.SeatNumber,
                BookingTime = ticket.BookingTime,
                Status = ticket.Status,
                OrderHistoryId = ticket.OrderHistoryId,
                IsDelete = ticket.IsDelete,
                CreateBy = ticket.CreateBy,
                CreateAt = ticket.CreateAt,
                UpdateBy = ticket.UpdateBy,
                UpdateAt = ticket.UpdateAt,
                DeleteBy = ticket.DeleteBy,
                DeleteAt = ticket.DeleteAt
            };
        }
        public IEnumerable<TicketDetailDto> GetByOrderHistoryIdWithDetails(int orderHistoryId)
        {
            var tickets = _ticketRepository.GetByOrderHistoryIdWithDetails(orderHistoryId);
            return tickets.Select(t => new TicketDetailDto
            {
                TicketId = t.TicketId,
                UserId = t.UserId,
                ShowtimeId = t.ShowtimeId,
                SeatNumber = t.SeatNumber,
                BookingTime = t.BookingTime,
                Status = t.Status,
                OrderHistoryId = t.OrderHistoryId,
                MovieTitle = t.Showtime.Movie.Title,
                RoomName = t.Showtime.Room.Name,
                CinemaName = t.Showtime.Room.Cinema.Name,
                ShowtimeStart = t.Showtime.StartTime
            });
        }

        public IEnumerable<TicketDetailDto> GetAllByOrderHistoryIdWithDetails(int orderHistoryId)
        {
            var tickets = _ticketRepository.GetAllByOrderHistoryIdWithDetails(orderHistoryId);
            return tickets.Select(t => new TicketDetailDto
            {
                TicketId = t.TicketId,
                UserId = t.UserId,
                ShowtimeId = t.ShowtimeId,
                SeatNumber = t.SeatNumber,
                BookingTime = t.BookingTime,
                Status = t.Status,
                OrderHistoryId = t.OrderHistoryId,
                MovieTitle = t.Showtime.Movie.Title,
                RoomName = t.Showtime.Room.Name,
                CinemaName = t.Showtime.Room.Cinema.Name,
                ShowtimeStart = t.Showtime.StartTime
            });
        }

        public async Task<IEnumerable<TicketDto>> GetByShowtimeIdAsync(int showtimeId)
        {
            var tickets = await _ticketRepository.GetByShowtimeIdAsync(showtimeId);
            return tickets.Select(t => new TicketDto
            {
                TicketId = t.TicketId,
                ShowtimeId = t.ShowtimeId,
                UserId = t.UserId,
                SeatNumber = t.SeatNumber,
                BookingTime = t.BookingTime,
                Status = t.Status,
                OrderHistoryId = t.OrderHistoryId,
                IsDelete = t.IsDelete,
                CreateBy = t.CreateBy,
                CreateAt = t.CreateAt,
                UpdateBy = t.UpdateBy,
                UpdateAt = t.UpdateAt,
                DeleteBy = t.DeleteBy,
                DeleteAt = t.DeleteAt
            });
        }

        public async Task<bool> RefundTicketAsync(int ticketId, decimal refundAmount, string refundReason, int userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null) return false;
            // Kiểm tra quyền sở hữu
            if (ticket.UserId != userId) return false;
            // Kiểm tra điều kiện hoàn vé (ví dụ: chưa quá giờ chiếu, chưa hoàn)
            if (ticket.Status == 2) return false; // Đã hoàn
            
            // Kiểm tra thời gian showtime để đảm bảo chưa quá giờ chiếu
            var showtime = ticket.Showtime;
            if (showtime != null && showtime.StartTime <= DateTime.Now)
            {
                return false; // Không thể hoàn vé sau khi đã chiếu
            }
            
            // Tính giá hoàn vé theo chính sách hiện tại
            var refundPercentage = await _refundPolicyService.GetCurrentRefundPercentageAsync();
            var originalPrice = showtime?.Price ?? 0;
            var calculatedRefundAmount = originalPrice * (refundPercentage / 100m);
            
            // Cập nhật trạng thái hoàn vé và thông tin hoàn vé
            ticket.Status = 2; // Đã hoàn
            ticket.RefundAmount = calculatedRefundAmount;
            ticket.RefundTime = DateTime.Now;
            ticket.RefundReason = refundReason;
            ticket.UpdateBy = userId.ToString();
            ticket.UpdateAt = DateTime.Now;
            
            // Xóa ticket để mở khóa ghế (hoặc có thể soft delete)
            ticket.IsDelete = true;
            ticket.DeleteAt = DateTime.Now;
            ticket.DeleteBy = userId.ToString();
            
            await _ticketRepository.UpdateAsync(ticket);
            
            // Lưu vào RefundHistory
            var refundHistoryDto = new RefundHistoryDto
            {
                TicketId = ticketId,
                RefundAmount = calculatedRefundAmount,
                RefundTime = DateTime.Now,
                Reason = refundReason,
                UserId = userId,
                CreateBy = userId.ToString(),
                CreateAt = DateTime.Now
            };
            await _refundHistoryService.AddAsync(refundHistoryDto);
            
            return true;
        }
    }
}