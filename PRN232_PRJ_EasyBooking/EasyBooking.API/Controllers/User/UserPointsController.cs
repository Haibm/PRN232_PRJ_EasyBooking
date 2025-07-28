using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.Interfaces;
using EasyBooking.Business.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EasyBooking.API.Controllers.User
{
    [ApiController]
    [Route("api/user/user-points")]
    [Authorize]
    public class UserPointsController : ControllerBase
    {
        private readonly IUserPointsService _userPointsService;

        public UserPointsController(IUserPointsService userPointsService)
        {
            _userPointsService = userPointsService;
        }

        [HttpGet("my-points")]
        public async Task<IActionResult> GetMyPoints()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var userPoints = await _userPointsService.GetByUserIdAsync(userId);
                
                if (userPoints == null)
                {
                    // Initialize user points if not exists
                    userPoints = await _userPointsService.InitializeUserPointsAsync(userId);
                }

                return Ok(userPoints);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyPoints: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("my-transactions")]
        public async Task<IActionResult> GetMyTransactions()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var currentPoints = await _userPointsService.GetUserCurrentPointsAsync(userId);
                var totalEarnedPoints = await _userPointsService.GetUserTotalEarnedPointsAsync(userId);
                var totalUsedPoints = await _userPointsService.GetUserTotalUsedPointsAsync(userId);

                var summary = new
                {
                    CurrentPoints = currentPoints,
                    TotalEarnedPoints = totalEarnedPoints,
                    TotalUsedPoints = totalUsedPoints
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyTransactions: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User.FindFirst("userId")?.Value
                ?? User.FindFirst("sub")?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }

            throw new InvalidOperationException("User ID not found in token");
        }
    }
} 