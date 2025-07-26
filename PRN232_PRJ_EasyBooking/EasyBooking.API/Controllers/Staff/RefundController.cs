using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace EasyBooking.API.Controllers.Staff
{
    [Route("api/staff/refunds")]
    [ApiController]
    public class RefundController : ControllerBase
    {
        private readonly IRefundHistoryService _refundHistoryService;
        private readonly IMovieService _movieService;
        private readonly ICinemaService _cinemaService;
        
        public RefundController(
            IRefundHistoryService refundHistoryService,
            IMovieService movieService,
            ICinemaService cinemaService)
        {
            _refundHistoryService = refundHistoryService;
            _movieService = movieService;
            _cinemaService = cinemaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int? movieId,
            [FromQuery] int? cinemaId)
        {
            var refunds = await _refundHistoryService.GetAllAsync();
            var refundList = refunds.ToList();
            
            // Lọc theo ngày
            if (fromDate.HasValue)
            {
                refundList = refundList.Where(r => r.RefundTime.Date >= fromDate.Value.Date).ToList();
            }
            
            if (toDate.HasValue)
            {
                refundList = refundList.Where(r => r.RefundTime.Date <= toDate.Value.Date).ToList();
            }
            
            // Lọc theo phim
            if (movieId.HasValue)
            {
                refundList = refundList.Where(r => r.MovieTitle != null && 
                    r.MovieTitle.Contains(movieId.Value.ToString())).ToList();
            }
            
            // Lọc theo rạp
            if (cinemaId.HasValue)
            {
                refundList = refundList.Where(r => r.CinemaName != null && 
                    r.CinemaName.Contains(cinemaId.Value.ToString())).ToList();
            }
            
            return Ok(refundList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var refund = await _refundHistoryService.GetByIdAsync(id);
            if (refund == null) return NotFound();
            return Ok(refund);
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            var refunds = await _refundHistoryService.GetAllAsync();
            var refundList = refunds.ToList();
            
            var statistics = new
            {
                TotalRefunds = refundList.Count,
                TotalRefundAmount = refundList.Sum(r => r.RefundAmount),
                TodayRefunds = refundList.Count(r => r.RefundTime.Date == System.DateTime.Today),
                AverageRefundAmount = refundList.Any() ? refundList.Average(r => r.RefundAmount) : 0,
                RefundsByMonth = refundList.GroupBy(r => new { r.RefundTime.Year, r.RefundTime.Month })
                    .Select(g => new { Month = $"{g.Key.Year}-{g.Key.Month:D2}", Count = g.Count(), Amount = g.Sum(r => r.RefundAmount) })
                    .OrderByDescending(x => x.Month)
                    .Take(12)
            };
            
            return Ok(statistics);
        }
    }
} 