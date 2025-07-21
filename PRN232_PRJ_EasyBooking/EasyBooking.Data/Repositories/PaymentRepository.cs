using EasyBooking.Business.Interfaces;
using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;

namespace EasyBooking.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CinemaBookingDbContext _context;
        public PaymentRepository(CinemaBookingDbContext context) { _context = context; }

        public int CreatePayment(Payment payment)
        {
            _context.Payments.Add(payment);
            _context.SaveChanges();
            return payment.PaymentId;
        }

        public void UpdateStatus(int paymentId, bool status, string responseCode = null, string transactionId = null)
        {
            var payment = _context.Payments.Find(paymentId);
            if (payment != null)
            {
                payment.Status = status;
                //payment.VnPayResponseCode = responseCode;
                payment.TransactionId = transactionId;
                _context.SaveChanges();
            }
        }

        public void GetById(int paymentId) => _context.Payments.Find(paymentId);

        public Payment GetByTransactionId(string transactionId)
        {
            // So sánh transactionId là tiền tố của vnp_TxnRef
            return _context.Payments.FirstOrDefault(p => transactionId.StartsWith(p.TransactionId));
        }
    }
} 