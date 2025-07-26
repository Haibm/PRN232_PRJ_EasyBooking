using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EasyBooking.Business.Interfaces;
using EasyBooking.Business.DTOs;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EasyBooking.API.Controllers.Staff
{
    [Route("api/staff/refund-policies")]
    [ApiController]
    [Authorize] // Chỉ yêu cầu JWT token
    public class RefundPolicyController : ControllerBase
    {
        private readonly IRefundPolicyService _refundPolicyService;
        
        public RefundPolicyController(IRefundPolicyService refundPolicyService)
        {
            _refundPolicyService = refundPolicyService;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            Console.WriteLine("Test endpoint called");
            return Ok(new { message = "RefundPolicy API is working!", timestamp = DateTime.Now });
        }

        [HttpGet("debug")]
        public async Task<IActionResult> Debug()
        {
            try
            {
                Console.WriteLine("Debug endpoint called");
                var policies = await _refundPolicyService.GetAllAsync();
                var count = policies.Count();
                Console.WriteLine($"Found {count} policies in database");
                
                var activePolicy = await _refundPolicyService.GetActivePolicyAsync();
                var activePolicyInfo = activePolicy != null ? 
                    $"Active: {activePolicy.PolicyName} ({activePolicy.RefundPercentage}%)" : 
                    "No active policy";
                
                return Ok(new { 
                    message = "Debug info", 
                    totalPolicies = count,
                    activePolicy = activePolicyInfo,
                    timestamp = DateTime.Now 
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Debug: {ex.Message}");
                return StatusCode(500, new { error = "Database error", message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                Console.WriteLine("GetAll called");
                var policies = await _refundPolicyService.GetAllAsync();
                Console.WriteLine($"Found {policies.Count()} policies");
                return Ok(policies);
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
                Console.WriteLine($"GetById called with id: {id}");
                var policy = await _refundPolicyService.GetByIdAsync(id);
                if (policy == null)
                {
                    Console.WriteLine($"Policy with id {id} not found");
                    return NotFound(new { error = "Policy not found" });
                }
                Console.WriteLine($"Found policy: {policy.PolicyName}");
                return Ok(policy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetById: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActivePolicy()
        {
            try
            {
                var policy = await _refundPolicyService.GetActivePolicyAsync();
                if (policy == null)
                {
                    return Ok(new { message = "No active policy found", data = (RefundPolicyDto)null });
                }
                return Ok(policy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetActivePolicy: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RefundPolicyDto policyDto)
        {
            try
            {
                var username = User.FindFirst("username")?.Value 
                    ?? User.FindFirst("name")?.Value 
                    ?? User.Identity?.Name 
                    ?? "Unknown";
                Console.WriteLine($"Create called by {username} with PolicyName: {policyDto?.PolicyName}, RefundPercentage: {policyDto?.RefundPercentage}");
                
                if (!ModelState.IsValid)
                {
                    Console.WriteLine($"ModelState errors: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                    return BadRequest(ModelState);
                }
                
                if (string.IsNullOrWhiteSpace(policyDto.PolicyName))
                {
                    Console.WriteLine("PolicyName is empty");
                    return BadRequest(new { error = "PolicyName is required" });
                }
                
                if (policyDto.RefundPercentage < 0 || policyDto.RefundPercentage > 100)
                {
                    Console.WriteLine($"Invalid RefundPercentage: {policyDto.RefundPercentage}");
                    return BadRequest(new { error = "RefundPercentage must be between 0 and 100" });
                }
                
                // Nếu policy mới được kích hoạt, tắt tất cả policy khác
                if (policyDto.IsActive)
                {
                    var allPolicies = await _refundPolicyService.GetAllAsync();
                    foreach (var policy in allPolicies)
                    {
                        if (policy.IsActive)
                        {
                            // Lấy lại entity từ DB để tránh EF tracking conflict
                            var dbPolicy = await _refundPolicyService.GetByIdAsync(policy.RefundPolicyId);
                            if (dbPolicy != null)
                            {
                                dbPolicy.IsActive = false;
                                dbPolicy.UpdateAt = DateTime.Now;
                                dbPolicy.UpdateBy = "Staff";
                                await _refundPolicyService.UpdateAsync(dbPolicy);
                            }
                        }
                    }
                }
                
                policyDto.CreateAt = DateTime.Now;
                policyDto.CreateBy = username;
                await _refundPolicyService.AddAsync(policyDto);
                
                Console.WriteLine($"Policy created successfully with ID: {policyDto.RefundPolicyId}");
                return CreatedAtAction(nameof(GetById), new { id = policyDto.RefundPolicyId }, policyDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RefundPolicyDto policyDto)
        {
            try
            {
                var username = User.FindFirst("username")?.Value 
                    ?? User.FindFirst("name")?.Value 
                    ?? User.Identity?.Name 
                    ?? "Unknown";
                Console.WriteLine($"Update called by {username} with ID: {id}, PolicyName: {policyDto?.PolicyName}, RefundPercentage: {policyDto?.RefundPercentage}");
                
                if (!ModelState.IsValid)
                {
                    Console.WriteLine($"ModelState errors: {string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage))}");
                    return BadRequest(ModelState);
                }
                
                if (string.IsNullOrWhiteSpace(policyDto.PolicyName))
                {
                    Console.WriteLine("PolicyName is empty");
                    return BadRequest(new { error = "PolicyName is required" });
                }
                
                if (policyDto.RefundPercentage < 0 || policyDto.RefundPercentage > 100)
                {
                    Console.WriteLine($"Invalid RefundPercentage: {policyDto.RefundPercentage}");
                    return BadRequest(new { error = "RefundPercentage must be between 0 and 100" });
                }
                
                var existingPolicy = await _refundPolicyService.GetByIdAsync(id);
                if (existingPolicy == null)
                {
                    Console.WriteLine($"Policy with ID {id} not found");
                    return NotFound(new { error = "Policy not found" });
                }
                
                // Nếu policy được kích hoạt, tắt tất cả policy khác
                if (policyDto.IsActive)
                {
                    var allPolicies = await _refundPolicyService.GetAllAsync();
                    foreach (var policy in allPolicies)
                    {
                        if (policy.IsActive && policy.RefundPolicyId != id)
                        {
                            // Lấy lại entity từ DB để tránh EF tracking conflict
                            var dbPolicy = await _refundPolicyService.GetByIdAsync(policy.RefundPolicyId);
                            if (dbPolicy != null)
                            {
                                dbPolicy.IsActive = false;
                                dbPolicy.UpdateAt = DateTime.Now;
                                dbPolicy.UpdateBy = "Staff";
                                await _refundPolicyService.UpdateAsync(dbPolicy);
                            }
                        }
                    }
                }
                
                policyDto.RefundPolicyId = id;
                policyDto.UpdateAt = DateTime.Now;
                policyDto.UpdateBy = username;
                await _refundPolicyService.UpdateAsync(policyDto);
                
                Console.WriteLine($"Policy {id} updated successfully");
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                Console.WriteLine($"Activate called with id: {id}");
                var policy = await _refundPolicyService.GetByIdAsync(id);
                if (policy == null)
                {
                    Console.WriteLine($"Policy with id {id} not found");
                    return NotFound(new { error = "Policy not found" });
                }
                
                // Sử dụng service method để tránh tracking conflict
                await _refundPolicyService.ActivatePolicyAsync(id);
                
                Console.WriteLine($"Policy {id} activated successfully");
                return Ok(new { message = "Policy activated successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Activate: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                Console.WriteLine($"Delete called with id: {id}");
                var policy = await _refundPolicyService.GetByIdAsync(id);
                if (policy == null)
                {
                    Console.WriteLine($"Policy with id {id} not found");
                    return NotFound(new { error = "Policy not found" });
                }
                
                await _refundPolicyService.DeleteAsync(id);
                Console.WriteLine($"Policy {id} deleted successfully");
                return Ok(new { message = "Policy deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
    }
} 