using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EasyBooking.API.Controllers.User
{
    [Route("api/user/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null) return NotFound();
            return Ok(movie);
        }

        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetDetailById(int id)
        {
            var movieDetail = await _movieService.GetDetailByIdAsync(id);
            if (movieDetail == null) return NotFound();
            return Ok(movieDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _movieService.AddAsync(movieDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieDto movieDto)
        {
            if (id != movieDto.MovieId) return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _movieService.UpdateAsync(movieDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _movieService.DeleteAsync(id);
            return NoContent();
        }
    }
} 