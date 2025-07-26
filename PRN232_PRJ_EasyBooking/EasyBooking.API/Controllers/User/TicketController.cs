using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace EasyBooking.API.Controllers.User
{
    [Route("api/user/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IRefundHistoryService _refundHistoryService;
        public TicketController(ITicketService ticketService, IRefundHistoryService refundHistoryService)
        {
            _ticketService = ticketService;
            _refundHistoryService = refundHistoryService;
        }

        [HttpGet("by-order/{orderHistoryId}")]
        public IActionResult GetTicketsByOrder(int orderHistoryId)
        {
            var tickets = _ticketService.GetByOrderHistoryIdWithDetails(orderHistoryId);
            return Ok(tickets);
        }

        [HttpGet("refunds/by-order/{orderHistoryId}")]
        public async Task<IActionResult> GetRefundsByOrder(int orderHistoryId)
        {
            // Lấy userId từ JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            
            // Lấy tất cả RefundHistory của user này
            var allRefunds = await _refundHistoryService.GetByUserIdAsync(userId);
            
            // Lấy tất cả ticket của order này (bao gồm cả đã hoàn)
            var tickets = _ticketService.GetAllByOrderHistoryIdWithDetails(orderHistoryId);
            var ticketIds = tickets.Select(t => t.TicketId).ToList();
            
            // Lọc RefundHistory theo ticketIds của order này
            var filteredRefunds = allRefunds.Where(r => ticketIds.Contains(r.TicketId)).ToList();
            
            return Ok(filteredRefunds);
        }

        [HttpPost("{id}/refund")]
        public async Task<IActionResult> RefundTicket(int id, [FromBody] RefundTicketRequest request)
        {
            // Lấy userId từ JWT
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim.Value);
            var success = await _ticketService.RefundTicketAsync(id, 0, request.RefundReason, userId);
            if (!success) return BadRequest(new { message = "Không thể hoàn vé. Kiểm tra điều kiện hoàn vé hoặc quyền sở hữu." });
            return Ok(new { success = true });
        }

        public class RefundTicketRequest
        {
            public string RefundReason { get; set; }
        }
    }
} 