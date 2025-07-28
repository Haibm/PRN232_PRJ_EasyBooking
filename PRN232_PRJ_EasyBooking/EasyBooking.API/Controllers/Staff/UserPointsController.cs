using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.Interfaces;
using EasyBooking.Business.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EasyBooking.API.Controllers.Staff
{
    [ApiController]
    [Route("api/staff/user-points")]
    [Authorize]
    public class UserPointsController : ControllerBase
    {
        private readonly IUserPointsService _userPointsService;
        private readonly IPointTransactionService _pointTransactionService;

        public UserPointsController(IUserPointsService userPointsService, IPointTransactionService pointTransactionService)
        {
            _userPointsService = userPointsService;
            _pointTransactionService = pointTransactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userPoints = await _userPointsService.GetAllAsync();
                return Ok(userPoints);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAll: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var userPoints = await _userPointsService.GetByIdAsync(id);
                if (userPoints == null)
                {
                    return NotFound(new { error = "User points not found" });
                }
                return Ok(userPoints);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetById: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            try
            {
                var userPoints = await _userPointsService.GetByUserIdAsync(userId);
                if (userPoints == null)
                {
                    return NotFound(new { error = "User points not found" });
                }
                return Ok(userPoints);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByUserId: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost("add-points")]
        public async Task<IActionResult> AddPoints([FromBody] AddPointsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { error = "Invalid request data" });
                }

                var username = User.FindFirst("username")?.Value
                    ?? User.FindFirst("name")?.Value
                    ?? User.Identity?.Name
                    ?? "Staff";

                Console.WriteLine($"AddPoints called by {username} for UserId: {request.UserId}, Points: {request.Points}");

                var result = await _userPointsService.AddPointsAsync(
                    request.UserId, 
                    request.Points, 
                    request.Description, 
                    null, 
                    username);

                Console.WriteLine($"Points added successfully for UserId: {request.UserId}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddPoints: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("user/{userId}/transactions")]
        public async Task<IActionResult> GetUserTransactions(int userId)
        {
            try
            {
                var transactions = await _pointTransactionService.GetByUserIdAsync(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserTransactions: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics()
        {
            try
            {
                var allUserPoints = await _userPointsService.GetAllAsync();
                var userPointsList = allUserPoints.ToList();

                var statistics = new
                {
                    TotalUsers = userPointsList.Count,
                    TotalEarnedPoints = userPointsList.Sum(up => up.TotalEarnedPoints),
                    TotalUsedPoints = userPointsList.Sum(up => up.TotalUsedPoints),
                    TotalCurrentPoints = userPointsList.Sum(up => up.CurrentPoints)
                };

                return Ok(statistics);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetStatistics: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }

    public class AddPointsRequest
    {
        public int UserId { get; set; }
        public int Points { get; set; }
        public string Description { get; set; } = "";
    }
} 