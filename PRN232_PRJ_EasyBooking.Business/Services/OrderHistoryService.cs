using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;

namespace EasyBooking.Business.Services
{
    public class OrderHistoryService : IOrderHistoryService
    {
        private readonly IOrderHistoryRepository _repo;
        public OrderHistoryService(IOrderHistoryRepository repo) { _repo = repo; }
        public void Add(OrderHistoryDto dto)
        {
            var entity = new OrderHistory
            {
                OrderHistoryId = dto.OrderHistoryId,
                PaymentId = dto.PaymentId,
                UserId = dto.UserId,
                Timestamp = dto.Timestamp,
                OrderNote = dto.OrderNote,
                OrderStatus = dto.OrderStatus
            };
            _repo.Add(entity);
        }
        public int GetLatestIdByPaymentAndUser(int paymentId, int userId)
        {
            return _repo.GetLatestIdByPaymentAndUser(paymentId, userId);
        }
    }
} 