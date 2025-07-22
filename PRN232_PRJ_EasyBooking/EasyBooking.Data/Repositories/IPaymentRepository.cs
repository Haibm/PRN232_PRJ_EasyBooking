using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Interfaces
{
    public interface IPaymentRepository
    {
        int CreatePayment(Payment payment);
        void UpdateStatus(int paymentId, bool status, string responseCode = null, string transactionId = null);
        void GetById(int paymentId);
        Payment GetByTransactionId(string transactionId);
        IEnumerable<Payment> GetAll();
    }
}