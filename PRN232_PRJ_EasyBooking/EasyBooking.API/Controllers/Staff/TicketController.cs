using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EasyBooking.API.Controllers.Staff
{
    [Route("api/staff/tickets")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _ticketService.GetAllAsync();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _ticketService.GetByIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TicketDto ticketDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _ticketService.AddAsync(ticketDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TicketDto ticketDto)
        {
            if (id != ticketDto.TicketId) return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _ticketService.UpdateAsync(ticketDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ticketService.DeleteAsync(id);
            return NoContent();
        }
    }
}