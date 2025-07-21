using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EasyBooking.API.Controllers.Staff
{
    [Route("api/staff")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;
        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        // Lấy tất cả ghế
        [HttpGet("seats")]
        public async Task<IActionResult> GetAll()
        {
            var seats = await _seatService.GetAllAsync();
            return Ok(seats);
        }

        // Lấy ghế theo room
        [HttpGet("rooms/{roomId}/seats")]
        public async Task<IActionResult> GetByRoom(int roomId)
        {
            var seats = await _seatService.GetByRoomIdAsync(roomId);
            return Ok(seats);
        }

        // Lấy ghế theo ID
        [HttpGet("seats/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seat = await _seatService.GetByIdAsync(id);
            if (seat == null) return NotFound();
            return Ok(seat);
        }

        // Thêm ghế
        [HttpPost("seats")]
        public async Task<IActionResult> Create([FromBody] SeatDto seatDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                await _seatService.AddAsync(seatDto);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Cập nhật ghế
        [HttpPut("seats/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SeatDto seatDto)
        {
            if (id != seatDto.SeatId) return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _seatService.UpdateAsync(seatDto);
            return NoContent();
        }

        // Xoá ghế
        [HttpDelete("seats/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _seatService.DeleteAsync(id);
            return NoContent();
        }
    }
}
