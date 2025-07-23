using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace EasyBooking.API.Controllers.User
{
    [Route("api/user/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("by-order/{orderHistoryId}")]
        public IActionResult GetTicketsByOrder(int orderHistoryId)
        {
            var tickets = _ticketService.GetByOrderHistoryIdWithDetails(orderHistoryId);
            return Ok(tickets);
        }
    }
} 