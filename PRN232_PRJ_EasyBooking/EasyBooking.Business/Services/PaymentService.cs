using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository) { _paymentRepository = paymentRepository; }

        public int CreatePayment(PaymentDto paymentDto)
        {
            // Map DTO sang entity
            var payment = new Payment
            {
                PaymentId = paymentDto.PaymentId,
                Amount = paymentDto.Amount,
                PaymentTime = paymentDto.PaymentTime,
                UserId = paymentDto.UserId,
                TransactionId = paymentDto.TransactionId,
                Status = paymentDto.Status,
                SeatsJson = paymentDto.SeatsJson,
                ShowtimeId = paymentDto.ShowtimeId
            };
            _paymentRepository.CreatePayment(payment);
            return payment.PaymentId;
        }
        public void UpdateStatus(int paymentId, bool status, string responseCode = null, string transactionId = null)
            => _paymentRepository.UpdateStatus(paymentId, status, responseCode, transactionId);
        public void GetById(int paymentId) => _paymentRepository.GetById(paymentId);

        public PaymentDto GetByTransactionId(string transactionId)
        {
            var payment = _paymentRepository.GetByTransactionId(transactionId);
            if (payment == null) return null;
            return new PaymentDto
            {
                PaymentId = payment.PaymentId,
                TransactionId = payment.TransactionId,
                Amount = payment.Amount,
                PaymentTime = payment.PaymentTime,
                UserId = payment.UserId,
                Status = payment.Status,
                SeatsJson = payment.SeatsJson,
                ShowtimeId = payment.ShowtimeId
                // ... các trường khác nếu cần ...
            };
        }

        public async Task<IEnumerable<PaymentDto>> GetAllAsync()
        {
            // Giả sử repository không async, dùng Task.Run để không block
            return await Task.Run(() =>
                _paymentRepository.GetAll().Select(payment => new PaymentDto
                {
                    PaymentId = payment.PaymentId,
                    TransactionId = payment.TransactionId,
                    Amount = payment.Amount,
                    PaymentTime = payment.PaymentTime,
                    UserId = payment.UserId,
                    Status = payment.Status,
                    SeatsJson = payment.SeatsJson,
                    ShowtimeId = payment.ShowtimeId
                })
            );
        }
    }
}