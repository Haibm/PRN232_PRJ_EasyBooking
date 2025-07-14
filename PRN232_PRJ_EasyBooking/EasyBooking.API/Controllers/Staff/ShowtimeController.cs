using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EasyBooking.API.Controllers.Staff
{
    [Route("api/staff/showtimes")]
    [ApiController]
    public class ShowtimeController : ControllerBase
    {
        private readonly IShowtimeService _showtimeService;
        public ShowtimeController(IShowtimeService showtimeService)
        {
            _showtimeService = showtimeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var showtimes = await _showtimeService.GetAllAsync();
            return Ok(showtimes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var showtime = await _showtimeService.GetByIdAsync(id);
            if (showtime == null) return NotFound();
            return Ok(showtime);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ShowtimeDto showtimeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _showtimeService.AddAsync(showtimeDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ShowtimeDto showtimeDto)
        {
            if (id != showtimeDto.ShowtimeId) return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _showtimeService.UpdateAsync(showtimeDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _showtimeService.DeleteAsync(id);
            return NoContent();
        }
    }
} 