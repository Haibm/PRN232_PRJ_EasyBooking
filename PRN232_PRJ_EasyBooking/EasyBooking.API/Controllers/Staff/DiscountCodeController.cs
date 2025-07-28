using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.Interfaces;
using EasyBooking.Business.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EasyBooking.API.Controllers.Staff
{
    [ApiController]
    [Route("api/staff/discount-codes")]
    [Authorize]
    public class DiscountCodeController : ControllerBase
    {
        private readonly IDiscountCodeService _discountCodeService;

        public DiscountCodeController(IDiscountCodeService discountCodeService)
        {
            _discountCodeService = discountCodeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var discountCodes = await _discountCodeService.GetAllAsync();
                return Ok(discountCodes);
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
                var discountCode = await _discountCodeService.GetByIdAsync(id);
                if (discountCode == null)
                {
                    return NotFound(new { error = "Discount code not found" });
                }
                return Ok(discountCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetById: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCodes()
        {
            try
            {
                var discountCodes = await _discountCodeService.GetActiveCodesAsync();
                return Ok(discountCodes);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetActiveCodes: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DiscountCodeDto discountCodeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { error = "Invalid request data" });
                }

                // Validation
                if (string.IsNullOrWhiteSpace(discountCodeDto.Code))
                {
                    return BadRequest(new { error = "Code is required" });
                }

                if (string.IsNullOrWhiteSpace(discountCodeDto.Name))
                {
                    return BadRequest(new { error = "Name is required" });
                }

                if (discountCodeDto.DiscountPercentage < 0 || discountCodeDto.DiscountPercentage > 100)
                {
                    return BadRequest(new { error = "Discount percentage must be between 0 and 100" });
                }

                if (discountCodeDto.DiscountAmount < 0)
                {
                    return BadRequest(new { error = "Discount amount cannot be negative" });
                }

                // Ensure at least one discount type is set
                if (discountCodeDto.DiscountPercentage == 0 && discountCodeDto.DiscountAmount == 0)
                {
                    return BadRequest(new { error = "Either discount percentage or discount amount must be greater than 0" });
                }

                if (discountCodeDto.RequiredPoints < 0)
                {
                    return BadRequest(new { error = "Required points cannot be negative" });
                }

                if (discountCodeDto.MaxUsage < 0)
                {
                    return BadRequest(new { error = "Max usage cannot be negative" });
                }

                if (discountCodeDto.StartDate >= discountCodeDto.EndDate)
                {
                    return BadRequest(new { error = "Start date must be before end date" });
                }

                // Set audit fields
                var username = User.FindFirst("username")?.Value
                    ?? User.FindFirst("name")?.Value
                    ?? User.Identity?.Name
                    ?? "Staff";

                discountCodeDto.CreateBy = username;
                discountCodeDto.CreateAt = DateTime.Now;

                var result = await _discountCodeService.AddAsync(discountCodeDto);
                return CreatedAtAction(nameof(GetById), new { id = result.DiscountCodeId }, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DiscountCodeDto discountCodeDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { error = "Invalid request data" });
                }

                if (id != discountCodeDto.DiscountCodeId)
                {
                    return BadRequest(new { error = "ID mismatch" });
                }

                // Validation
                if (string.IsNullOrWhiteSpace(discountCodeDto.Code))
                {
                    return BadRequest(new { error = "Code is required" });
                }

                if (string.IsNullOrWhiteSpace(discountCodeDto.Name))
                {
                    return BadRequest(new { error = "Name is required" });
                }

                if (discountCodeDto.DiscountPercentage < 0 || discountCodeDto.DiscountPercentage > 100)
                {
                    return BadRequest(new { error = "Discount percentage must be between 0 and 100" });
                }

                if (discountCodeDto.DiscountAmount < 0)
                {
                    return BadRequest(new { error = "Discount amount cannot be negative" });
                }

                // Ensure at least one discount type is set
                if (discountCodeDto.DiscountPercentage == 0 && discountCodeDto.DiscountAmount == 0)
                {
                    return BadRequest(new { error = "Either discount percentage or discount amount must be greater than 0" });
                }

                if (discountCodeDto.RequiredPoints < 0)
                {
                    return BadRequest(new { error = "Required points cannot be negative" });
                }

                if (discountCodeDto.MaxUsage < 0)
                {
                    return BadRequest(new { error = "Max usage cannot be negative" });
                }

                if (discountCodeDto.StartDate >= discountCodeDto.EndDate)
                {
                    return BadRequest(new { error = "Start date must be before end date" });
                }

                // Set audit fields
                var username = User.FindFirst("username")?.Value
                    ?? User.FindFirst("name")?.Value
                    ?? User.Identity?.Name
                    ?? "Staff";

                discountCodeDto.UpdateBy = username;
                discountCodeDto.UpdateAt = DateTime.Now;

                var result = await _discountCodeService.UpdateAsync(discountCodeDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _discountCodeService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }
} 