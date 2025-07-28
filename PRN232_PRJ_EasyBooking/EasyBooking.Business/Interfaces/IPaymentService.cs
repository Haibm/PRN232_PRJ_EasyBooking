using EasyBooking.Business.DTOs;
using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Interfaces
{
    public interface IPaymentService
    {
        int CreatePayment(PaymentDto paymentDto);
        Task<int> CreatePaymentAsync(PaymentDto paymentDto, string discountCode = null);
        void UpdateStatus(int paymentId, bool status, string responseCode = null, string transactionId = null);
        void GetById(int paymentId);
        PaymentDto GetByTransactionId(string transactionId);
        Task<IEnumerable<PaymentDto>> GetAllAsync();
    }
}