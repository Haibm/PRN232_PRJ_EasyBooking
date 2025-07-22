using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using System.Security.Claims;
using System.Text.Json;

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
        private readonly IOrderHistoryService _orderHistoryService;
        public OrderController(
            IOrderService orderService,
            IPaymentService paymentService,
            IVnPayService vnPayService,
            ITicketService ticketService,
            IOrderHistoryService orderHistoryService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _vnPayService = vnPayService;
            _ticketService = ticketService;
            _orderHistoryService = orderHistoryService;
        }

        public static Dictionary<string, OrderInitDto> OrderCache = new();

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

            // 2. Tạo payment (pending), lưu SeatsJson và ShowtimeId
            var paymentId = _paymentService.CreatePayment(new PaymentDto
            {
                TransactionId = transactionId,
                Amount = order.TotalPrice,
                UserId = userId,
                Status = false,
                PaymentTime = DateTime.Now,
                SeatsJson = JsonSerializer.Serialize(order.Seats),
                ShowtimeId = order.ShowtimeId
            });

            // 3. Tạo VnPaymentRequestDto
            var vnPayModel = new VnPaymentRequestDto
            {
                Amount = order.TotalPrice,
                CreatedDate = DateTime.Now,
                Description = $"Thanh toán vé xem phim - OrderId: {transactionId}",
                Id = transactionId
            };

            var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
            return Ok(new { paymentUrl });
        }



        [HttpGet("payment-callback")]
        public IActionResult PaymentCallBack()
        {
            var transactionId = Request.Query["vnp_TxnRef"].ToString();
            if (string.IsNullOrEmpty(transactionId))
                return Redirect("https://localhost:7150/user/paymentfail");
            var payment = _paymentService.GetByTransactionId(transactionId);
            if (payment == null)
                return Redirect("https://localhost:7150/user/paymentfail");

            var response = _vnPayService.PaymentExecute(Request.Query);
            if (response == null || response.VnPayReponseCode != "00")
            {
                _paymentService.UpdateStatus(payment.PaymentId, false, response?.VnPayReponseCode);
                return Redirect("https://localhost:7150/user/paymentfail");
            }

            bool isValidSignature = _vnPayService.ValidateSignature(Request.Query["vnp_SecureHash"], Request.Query);
            if (!isValidSignature)
            {
                _paymentService.UpdateStatus(payment.PaymentId, false, "InvalidSignature");
                return Redirect("https://localhost:7150/user/paymentfail");
            }

            _paymentService.UpdateStatus(payment.PaymentId, true, response.VnPayReponseCode, response.TransactionId);

            // Lấy danh sách ghế và showtimeId từ Payment để tạo ticket
            var seats = !string.IsNullOrEmpty(payment.SeatsJson)
                ? JsonSerializer.Deserialize<List<string>>(payment.SeatsJson)
                : new List<string>();
            // Lưu OrderHistory như cũ và lấy lại OrderHistoryId vừa tạo
            var orderHistoryDto = new OrderHistoryDto
            {
                PaymentId = payment.PaymentId,
                UserId = payment.UserId ?? 0,
                Timestamp = DateTime.Now,
                OrderStatus = "Success"
            };
            _orderHistoryService.Add(orderHistoryDto);
            // Lấy lại OrderHistoryId vừa tạo (giả sử lấy theo PaymentId và UserId mới nhất)
            var orderHistoryId = _orderHistoryService.GetLatestIdByPaymentAndUser(payment.PaymentId, payment.UserId ?? 0);
            // Tạo ticket và truyền OrderHistoryId
            foreach (var seat in seats)
            {
                var ticket = new TicketDto
                {
                    UserId = payment.UserId ?? 0,
                    ShowtimeId = payment.ShowtimeId ?? 0,
                    SeatNumber = seat,
                    BookingTime = DateTime.Now,
                    Status = 1,
                    OrderHistoryId = orderHistoryId
                };
                _ticketService.CreateTicket(ticket);
            }

            return Redirect("https://localhost:7150/user/paymentsuccess");
        }

        [HttpGet("PaymentFail")]
        public IActionResult PaymentFail() => Content("Thanh toán thất bại! Đã có lỗi xảy ra trong quá trình thanh toán. Vui lòng thử lại hoặc liên hệ hỗ trợ.");

        [HttpGet("PaymentSuccess")]
        public IActionResult PaymentSuccess() => Content("Thanh toán thành công! Cảm ơn bạn đã sử dụng dịch vụ.");
    }
}