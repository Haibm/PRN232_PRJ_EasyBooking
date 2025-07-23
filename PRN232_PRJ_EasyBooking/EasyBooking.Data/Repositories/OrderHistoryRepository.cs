using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;

namespace EasyBooking.Data.Repositories
{
    public class OrderHistoryRepository : IOrderHistoryRepository
    {
        private readonly CinemaBookingDbContext _context;
        public OrderHistoryRepository(CinemaBookingDbContext context) { _context = context; }
        public void Add(OrderHistory orderHistory)
        {
            _context.OrderHistories.Add(orderHistory);
            _context.SaveChanges();
        }
        public int GetLatestIdByPaymentAndUser(int paymentId, int userId)
        {
            var entity = _context.OrderHistories
                .Where(x => x.PaymentId == paymentId && x.UserId == userId)
                .OrderByDescending(x => x.OrderHistoryId)
                .FirstOrDefault();
            return entity?.OrderHistoryId ?? 0;
        }
        public IEnumerable<OrderHistory> GetByUserId(int userId)
        {
            return _context.OrderHistories.Where(x => x.UserId == userId).OrderByDescending(x => x.OrderHistoryId).ToList();
        }
    }
} 