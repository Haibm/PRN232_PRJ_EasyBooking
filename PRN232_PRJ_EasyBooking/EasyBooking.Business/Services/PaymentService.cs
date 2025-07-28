using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserPointsService _userPointsService;
        private readonly IDiscountCodeService _discountCodeService;
        private readonly IUserDiscountCodeService _userDiscountCodeService;
        private readonly IPointConfigService _pointConfigService;

        public PaymentService(
            IPaymentRepository paymentRepository, 
            IUserPointsService userPointsService,
            IDiscountCodeService discountCodeService,
            IUserDiscountCodeService userDiscountCodeService,
            IPointConfigService pointConfigService)
        { 
            _paymentRepository = paymentRepository;
            _userPointsService = userPointsService;
            _discountCodeService = discountCodeService;
            _userDiscountCodeService = userDiscountCodeService;
            _pointConfigService = pointConfigService;
        }

        public async Task<int> CreatePaymentAsync(PaymentDto paymentDto, string discountCode = null)
        {
            decimal finalAmount = paymentDto.Amount;
            int? orderId = null;

            // Xử lý mã giảm giá nếu có
            if (!string.IsNullOrEmpty(discountCode))
            {
                var discountAmount = await _discountCodeService.CalculateDiscountAsync(discountCode, paymentDto.Amount);
                finalAmount = paymentDto.Amount - discountAmount;
                
                // Sử dụng mã giảm giá
                await _discountCodeService.UseCodeAsync(discountCode, paymentDto.UserId ?? 0, paymentDto.PaymentId);
            }

            // Map DTO sang entity
            var payment = new Payment
            {
                PaymentId = paymentDto.PaymentId,
                Amount = finalAmount, // Sử dụng số tiền sau giảm giá
                PaymentTime = paymentDto.PaymentTime,
                UserId = paymentDto.UserId,
                TransactionId = paymentDto.TransactionId,
                Status = paymentDto.Status,
                SeatsJson = paymentDto.SeatsJson,
                ShowtimeId = paymentDto.ShowtimeId
            };
            
            _paymentRepository.CreatePayment(payment);
            orderId = payment.PaymentId;

            // Tích điểm cho user theo cấu hình
            var pointsToEarn = await _pointConfigService.CalculatePointsAsync(finalAmount);
            if (pointsToEarn > 0)
            {
                await _userPointsService.AddPointsAsync(
                    paymentDto.UserId ?? 0, 
                    pointsToEarn, 
                    $"Tích điểm từ đơn hàng #{orderId}", 
                    orderId, 
                    "System");
            }

            return payment.PaymentId;
        }

        public int CreatePayment(PaymentDto paymentDto)
        {
            return CreatePaymentAsync(paymentDto).Result;
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