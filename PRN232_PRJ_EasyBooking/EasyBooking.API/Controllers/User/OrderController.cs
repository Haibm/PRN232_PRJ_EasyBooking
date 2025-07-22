using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using System.Security.Claims;

namespace EasyBooking.API.Controllers.User
{
    [Route("api/user/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IVnPayService _vnPayService;
        private readonly ITicketService _ticketService;

        public OrderController(
            IOrderService orderService,
            IPaymentService paymentService,
            IVnPayService vnPayService,
            ITicketService ticketService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _vnPayService = vnPayService;
            _ticketService = ticketService;
        }

        [HttpPost("init")]
        public IActionResult InitOrder([FromBody] OrderInitDto order)
        {
            if (order == null || order.Seats == null || order.Seats.Count == 0)
                return BadRequest("Invalid order");
            return Ok(order);
        }

        [HttpPost("Payment")]
        public IActionResult Payment([FromBody] OrderInitDto order)
        {
            if (order == null)
                return BadRequest("Order is null");
            if (order.Seats == null || order.Seats.Count == 0)
                return BadRequest("Seats is null or empty");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);


            // 1. Tạo order
            var transactionId = _orderService.GenerateCodePayment();

            // 2. Tạo payment (pending)
            var paymentId = _paymentService.CreatePayment(new PaymentDto
            {
                TransactionId = transactionId,
                Amount = order.TotalPrice,
                UserId = userId,
                Status = false,
                PaymentTime = DateTime.Now
            });

            // 3. Tạo VnPaymentRequestDto
            var vnPayModel = new VnPaymentRequestDto
            {
                Amount = order.TotalPrice,
                CreatedDate = DateTime.Now,
                Description = $"Thanh toán vé xem phim - OrderId: {transactionId}",
                Id = transactionId
            };
            HttpContext.Session.SetInt32("PaymentId", paymentId);

            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
            return Ok(new { paymentUrl });
        }



        [HttpGet("payment-callback")]
        public IActionResult PaymentCallBack()
        {
            // Lấy transactionId từ vnp_TxnRef (chuỗi)
            var transactionId = Request.Query["vnp_TxnRef"].ToString();
            if (string.IsNullOrEmpty(transactionId))
                return Redirect("/payment-fail");
            var payment = _paymentService.GetByTransactionId(transactionId);
            if (payment == null)
                return Redirect("/payment-fail");

            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayReponseCode != "00")
            {
                _paymentService.UpdateStatus(payment.PaymentId, false, response?.VnPayReponseCode);
                return Redirect("/payment-fail");
            }

            bool isValidSignature = _vnPayService.ValidateSignature(Request.Query["vnp_SecureHash"], Request.Query);
            if (!isValidSignature)
            {
                _paymentService.UpdateStatus(payment.PaymentId, false, "InvalidSignature");
                return Redirect("/payment-fail");
            }

            _paymentService.UpdateStatus(payment.PaymentId, true, response.VnPayReponseCode, response.TransactionId);
            return Redirect("/payment-success");
        }

        [Route("/payment-fail")]
        [HttpGet]
        public IActionResult PaymentFail() => Content("Thanh toán thất bại! Đã có lỗi xảy ra trong quá trình thanh toán. Vui lòng thử lại hoặc liên hệ hỗ trợ.");

        [Route("/payment-success")]
        [HttpGet]
        public IActionResult PaymentSuccess() => Content("Thanh toán thành công! Cảm ơn bạn đã sử dụng dịch vụ.");
    }
}