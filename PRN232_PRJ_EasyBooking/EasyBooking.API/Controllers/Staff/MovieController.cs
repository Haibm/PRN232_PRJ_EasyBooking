using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EasyBooking.API.Controllers.Staff
{
    [Route("api/staff/movies")]
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieDto movieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            // Validation cho Status
            if (movieDto.Status.HasValue && (movieDto.Status.Value < 1 || movieDto.Status.Value > 3))
            {
                return BadRequest("Status phải là 1 (Đang chiếu), 2 (Sắp chiếu), hoặc 3 (Ngừng chiếu)");
            }
            
            // Validation cho PosterUrl
            if (!string.IsNullOrEmpty(movieDto.PosterUrl))
            {
                if (movieDto.PosterUrl.StartsWith("data:image"))
                {
                    if (movieDto.PosterUrl.Length > 3500)
                    {
                        return BadRequest("Base64 string quá dài. Vui lòng sử dụng hình ảnh nhỏ hơn hoặc URL thay vì base64.");
                    }
                }
                else if (movieDto.PosterUrl.Length > 500)
                {
                    return BadRequest("URL quá dài. Vui lòng sử dụng URL ngắn hơn.");
                }
            }
            
            await _movieService.AddAsync(movieDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MovieDto movieDto)
        {
            if (id != movieDto.MovieId) return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            // Validation cho Status
            if (movieDto.Status.HasValue && (movieDto.Status.Value < 1 || movieDto.Status.Value > 3))
            {
                return BadRequest("Status phải là 1 (Đang chiếu), 2 (Sắp chiếu), hoặc 3 (Ngừng chiếu)");
            }
            
            // Validation cho PosterUrl
            if (!string.IsNullOrEmpty(movieDto.PosterUrl))
            {
                if (movieDto.PosterUrl.StartsWith("data:image"))
                {
                    if (movieDto.PosterUrl.Length > 3500)
                    {
                        return BadRequest("Base64 string quá dài. Vui lòng sử dụng hình ảnh nhỏ hơn hoặc URL thay vì base64.");
                    }
                }
                else if (movieDto.PosterUrl.Length > 500)
                {
                    return BadRequest("URL quá dài. Vui lòng sử dụng URL ngắn hơn.");
                }
            }
            
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