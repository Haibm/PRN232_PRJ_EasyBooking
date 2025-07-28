using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.Interfaces;
using EasyBooking.Business.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EasyBooking.API.Controllers.User
{
    [ApiController]
    [Route("api/user/discount-codes")]
    [Authorize]
    public class DiscountCodeController : ControllerBase
    {
        private readonly IDiscountCodeService _discountCodeService;
        private readonly IUserDiscountCodeService _userDiscountCodeService;
        private readonly IUserPointsService _userPointsService;

        public DiscountCodeController(
            IDiscountCodeService discountCodeService,
            IUserDiscountCodeService userDiscountCodeService,
            IUserPointsService userPointsService)
        {
            _discountCodeService = discountCodeService;
            _userDiscountCodeService = userDiscountCodeService;
            _userPointsService = userPointsService;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableCodes()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var discountCodes = await _discountCodeService.GetAvailableCodesAsync();
                return Ok(discountCodes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAvailableCodes: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("my-codes")]
        public async Task<IActionResult> GetMyCodes()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var userCodes = await _userDiscountCodeService.GetByUserIdAsync(userId);
                return Ok(userCodes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyCodes: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("my-unused-codes")]
        public async Task<IActionResult> GetMyUnusedCodes()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var unusedCodes = await _userDiscountCodeService.GetUnusedByUserIdAsync(userId);
                return Ok(unusedCodes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMyUnusedCodes: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost("redeem/{discountCodeId}")]
        public async Task<IActionResult> RedeemCode(int discountCodeId)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _userDiscountCodeService.RedeemCodeAsync(userId, discountCodeId, 0);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RedeemCode: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost("calculate-discount")]
        public async Task<IActionResult> CalculateDiscount([FromBody] CalculateDiscountRequest request)
        {
            try
            {
                var discountAmount = await _discountCodeService.CalculateDiscountAsync(request.Code, request.OriginalAmount);
                var finalAmount = request.OriginalAmount - discountAmount;

                return Ok(new
                {
                    discountAmount = discountAmount,
                    finalAmount = finalAmount
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CalculateDiscount: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost("use-code")]
        public async Task<IActionResult> UseCode([FromBody] UseCodeRequest request)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _userDiscountCodeService.UseCodeAsync(request.UserDiscountCodeId, request.OrderId);
                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UseCode: {ex.Message}");
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

    public class CalculateDiscountRequest
    {
        public string Code { get; set; } = "";
        public decimal OriginalAmount { get; set; }
    }

    public class UseCodeRequest
    {
        public int UserDiscountCodeId { get; set; }
        public int OrderId { get; set; }
    }
} 