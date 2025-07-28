using Microsoft.AspNetCore.Mvc;
using EasyBooking.Business.Interfaces;
using EasyBooking.Business.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using EasyBooking.Data.DbContexts;

namespace EasyBooking.API.Controllers.Staff
{
    [ApiController]
    [Route("api/staff/point-configs")]
    [Authorize]
    public class PointConfigController : ControllerBase
    {
        private readonly IPointConfigService _pointConfigService;
        private readonly CinemaBookingDbContext _context;

        public PointConfigController(IPointConfigService pointConfigService, CinemaBookingDbContext context)
        {
            _pointConfigService = pointConfigService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var configs = await _pointConfigService.GetAllAsync();
                return Ok(configs);
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
                var config = await _pointConfigService.GetByIdAsync(id);
                if (config == null)
                {
                    return NotFound(new { error = "Point config not found" });
                }
                return Ok(config);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetById: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveConfig()
        {
            try
            {
                var config = await _pointConfigService.GetActiveConfigAsync();
                if (config == null)
                {
                    return NotFound(new { error = "No active point config found" });
                }
                return Ok(config);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetActiveConfig: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PointConfigDto pointConfigDto)
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

                pointConfigDto.CreateBy = username;
                pointConfigDto.CreateAt = DateTime.Now;

                var result = await _pointConfigService.AddAsync(pointConfigDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PointConfigDto pointConfigDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { error = "Invalid request data" });
                }

                var existingConfig = await _pointConfigService.GetByIdAsync(id);
                if (existingConfig == null)
                {
                    return NotFound(new { error = "Point config not found" });
                }

                var username = User.FindFirst("username")?.Value
                    ?? User.FindFirst("name")?.Value
                    ?? User.Identity?.Name
                    ?? "Staff";

                pointConfigDto.PointConfigId = id;
                pointConfigDto.UpdateBy = username;
                pointConfigDto.UpdateAt = DateTime.Now;

                var result = await _pointConfigService.UpdateAsync(pointConfigDto);
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
                var existingConfig = await _pointConfigService.GetByIdAsync(id);
                if (existingConfig == null)
                {
                    return NotFound(new { error = "Point config not found" });
                }

                await _pointConfigService.DeleteAsync(id);
                return Ok(new { message = "Point config deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculatePoints([FromBody] CalculatePointsRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { error = "Invalid request data" });
                }

                var points = await _pointConfigService.CalculatePointsAsync(request.Amount);
                return Ok(new { amount = request.Amount, points = points });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CalculatePoints: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("simple-test")]
        [AllowAnonymous]
        public async Task<IActionResult> SimpleTest()
        {
            try
            {
                Console.WriteLine("SimpleTest called");
                
                // Test 1: Kiểm tra context
                if (_pointConfigService == null)
                {
                    return StatusCode(500, new { error = "Service is null" });
                }
                
                // Test 2: Kiểm tra database connection bằng cách đếm records
                var count = await _pointConfigService.GetAllAsync();
                var countResult = count.Count();
                
                Console.WriteLine($"SimpleTest: Found {countResult} records");
                
                return Ok(new { 
                    message = "Database connection successful", 
                    recordCount = countResult,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SimpleTest error: {ex.Message}");
                Console.WriteLine($"SimpleTest stack trace: {ex.StackTrace}");
                return StatusCode(500, new { 
                    error = "Database connection failed", 
                    message = ex.Message,
                    timestamp = DateTime.Now
                });
            }
        }

        [HttpGet("raw-test")]
        [AllowAnonymous]
        public async Task<IActionResult> RawTest()
        {
            try
            {
                Console.WriteLine("RawTest called");
                
                // Test raw SQL query
                var sql = "SELECT COUNT(*) FROM PointConfig";
                var count = await _context.Database.ExecuteSqlRawAsync(sql);
                
                Console.WriteLine($"RawTest: Raw SQL count = {count}");
                
                return Ok(new { 
                    message = "Raw SQL test successful", 
                    count = count,
                    timestamp = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RawTest error: {ex.Message}");
                Console.WriteLine($"RawTest stack trace: {ex.StackTrace}");
                return StatusCode(500, new { 
                    error = "Raw SQL test failed", 
                    message = ex.Message,
                    timestamp = DateTime.Now
                });
            }
        }
    }

    public class CalculatePointsRequest
    {
        public decimal Amount { get; set; }
    }
} 