using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBooking.Business.DTOs
{
    public class VnPaymentResponseDto
    {
        public bool Success { get; set; }
        public string? PaymentMethod { get; set; }
        public string? OrderDescription { get; set; }
        public string? OrderId { get; set; }
        public int Amount { get; set; }
        public string? PaymentId { get; set; }
        public string? TransactionId { get; set; }
        public string? Token { get; set; }
        public string? VnPayReponseCode { get; set; }
    }
}
