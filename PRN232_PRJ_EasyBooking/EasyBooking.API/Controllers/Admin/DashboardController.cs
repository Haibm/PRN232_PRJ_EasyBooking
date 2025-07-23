using EasyBooking.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic; // Added for List

namespace EasyBooking.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;
        private readonly IShowtimeService _showtimeService; 
        public DashboardController(
            IMovieService movieService,
            IUserService userService,
            IPaymentService paymentService,
            IShowtimeService showtimeService) 
        {
            _movieService = movieService;
            _userService = userService;
            _paymentService = paymentService;
            _showtimeService = showtimeService; 
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var totalMovies = await _movieService.GetTotalMoviesAsync();
            var totalUsersBooked = await _userService.GetTotalUsersBookedAsync();
            var totalPaid = (await _paymentService.GetAllAsync()).Where(p => p.Status== true).Sum(p => p.Amount);
            var topSpender = await _userService.GetTopSpenderAsync();
            var topMovie = await _movieService.GetTopMovieAsync();
            return Ok(new
            {
                totalMovies,
                totalUsersBooked,
                totalPaid,
                topSpender = topSpender != null ? new { topSpender.UserId, topSpender.FullName } : null,
                topMovie = topMovie != null ? new { topMovie.MovieId, topMovie.Title } : null
            });
        }

        [HttpGet("moviebuyers")]
        public async Task<IActionResult> GetMovieBuyers([FromQuery] string filter = null)
        {
            var movies = await _movieService.GetAllAsync();
            var payments = (await _paymentService.GetAllAsync()).Where(p => p.Status == true);
            var users = await _userService.GetAllAsync();
            var showtimes = await _showtimeService.GetAllAsync();

            var movieBuyerList = movies.Select(movie => {
                var showtimeIds = showtimes.Where(s => s.MovieId == movie.MovieId).Select(s => s.ShowtimeId).ToList();
                var buyers = payments
                    .Where(p => showtimeIds.Contains(p.ShowtimeId ?? 0))
                    .GroupBy(p => p.UserId)
                    .Select(g => {
                        var user = users.FirstOrDefault(u => u.UserId == g.Key);
                        return new {
                            userId = g.Key ?? 0,
                            fullName = user?.FullName ?? "Unknown",
                            totalPaid = g.Sum(x => x.Amount)
                        };
                    }).Where(b => b.totalPaid > 0).ToList();
                return new {
                    movieId = movie.MovieId,
                    movieTitle = movie.Title,
                    buyers = buyers
                };
            }).Where(mb => mb.buyers.Count > 0).ToList();

            // Top spender
            var topSpender = payments
                .GroupBy(p => p.UserId)
                .Select(g => new {
                    userId = g.Key ?? 0,
                    fullName = users.FirstOrDefault(u => u.UserId == g.Key)?.FullName ?? "Unknown",
                    totalPaid = g.Sum(x => x.Amount)
                })
                .OrderByDescending(x => x.totalPaid)
                .FirstOrDefault();

            // Most purchased movie
            var mostPurchased = movieBuyerList
                .OrderByDescending(mb => mb.buyers.Count)
                .FirstOrDefault();

            // Filter logic
            object filteredMovieBuyers = movieBuyerList;
            if (filter == "topspender" && topSpender != null)
            {
                filteredMovieBuyers = movieBuyerList.Where(mb => mb.buyers.Any(b => b.userId == topSpender.userId)).ToList();
            }
            else if (filter == "mostpurchased" && mostPurchased != null)
            {
                filteredMovieBuyers = new List<object> { mostPurchased };
            }

            return Ok(new
            {
                movieBuyers = filteredMovieBuyers,
                topSpender,
                mostPurchasedMovie = mostPurchased
            });
        }
    }
} 