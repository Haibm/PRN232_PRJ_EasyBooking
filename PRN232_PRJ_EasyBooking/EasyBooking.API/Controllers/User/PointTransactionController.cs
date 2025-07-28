using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.Interfaces;
using EasyBooking.Business.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EasyBooking.API.Controllers.User
{
    [ApiController]
    [Route("api/user/point-transactions")]
    [Authorize]
    public class PointTransactionController : ControllerBase
    {
        private readonly IPointTransactionService _pointTransactionService;

        public PointTransactionController(IPointTransactionService pointTransactionService)
        {
            _pointTransactionService = pointTransactionService;
        }

        [HttpGet("my-transactions")]
        public async Task<IActionResult> GetMyTransactions()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var transactions = await _pointTransactionService.GetByUserIdAsync(userId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyTransactions: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("my-transactions/{transactionType}")]
        public async Task<IActionResult> GetMyTransactionsByType(string transactionType)
        {
            try
            {
                if (!IsValidTransactionType(transactionType))
                {
                    return BadRequest(new { error = "Invalid transaction type" });
                }

                var userId = GetUserIdFromToken();
                var transactions = await _pointTransactionService.GetByUserIdAndTypeAsync(userId, transactionType);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyTransactionsByType: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("transaction-summary")]
        public async Task<IActionResult> GetTransactionSummary()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var currentPoints = await _pointTransactionService.GetUserTotalPointsAsync(userId);
                var earnedPoints = await _pointTransactionService.GetUserEarnedPointsAsync(userId);
                var usedPoints = await _pointTransactionService.GetUserUsedPointsAsync(userId);

                var summary = new
                {
                    CurrentPoints = currentPoints,
                    TotalEarnedPoints = earnedPoints,
                    TotalUsedPoints = usedPoints
                };

                return Ok(summary);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTransactionSummary: {ex.Message}");
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

        private bool IsValidTransactionType(string transactionType)
        {
            return transactionType switch
            {
                "EARN" => true,
                "USE" => true,
                "REDEEM" => true,
                _ => false
            };
        }
    }
} 