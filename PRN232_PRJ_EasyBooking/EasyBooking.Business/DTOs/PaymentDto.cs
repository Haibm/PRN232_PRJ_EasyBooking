namespace EasyBooking.Business.DTOs
{
    public class PaymentDto
    {
        public string? Id { get; set; }
        public int PaymentId { get; set; }

        public int? TicketId { get; set; }

        public decimal Amount { get; set; }

        public DateTime? PaymentTime { get; set; }

        public int? UserId { get; set; }

        public string? TransactionId { get; set; }

        public bool? Status { get; set; }
    }
} 