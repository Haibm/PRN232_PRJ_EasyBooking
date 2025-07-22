using EasyBooking.Business.DTOs;
using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Interfaces
{
    public interface IPaymentService
    {
        int CreatePayment(PaymentDto paymentDto);
        void UpdateStatus(int paymentId, bool status, string responseCode = null, string transactionId = null);
        void GetById(int paymentId);
        PaymentDto GetByTransactionId(string transactionId);
    }
}