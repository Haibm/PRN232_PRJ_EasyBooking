using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EasyBooking.API.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new { message = "Public endpoint works", timestamp = DateTime.Now });
        }

        [HttpGet("auth")]
        [Authorize]
        public IActionResult Auth()
        {
            var username = User.FindFirst("username")?.Value;
            var role = User.FindFirst("role")?.Value;
            
            return Ok(new { 
                message = "Auth endpoint works", 
                username = username,
                role = role,
                timestamp = DateTime.Now 
            });
        }
    }
} 