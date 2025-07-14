using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EasyBooking.API.Controllers.Staff
{
    [Route("api/staff/cinemas")]
    [ApiController]
    public class CinemaController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;
        public CinemaController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cinemas = await _cinemaService.GetAllAsync();
            return Ok(cinemas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cinema = await _cinemaService.GetByIdAsync(id);
            if (cinema == null) return NotFound();
            return Ok(cinema);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CinemaDto cinemaDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _cinemaService.AddAsync(cinemaDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CinemaDto cinemaDto)
        {
            if (id != cinemaDto.CinemaId) return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _cinemaService.UpdateAsync(cinemaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cinemaService.DeleteAsync(id);
            return NoContent();
        }
    }
} 