using EasyBooking.Data.Entities;

namespace EasyBooking.Data.Repositories
{
    public interface IOrderHistoryRepository
    {
        void Add(OrderHistory orderHistory);
        int GetLatestIdByPaymentAndUser(int paymentId, int userId);
        IEnumerable<OrderHistory> GetByUserId(int userId);
    }
} 