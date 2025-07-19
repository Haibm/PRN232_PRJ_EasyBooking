//using EasyBooking.Business.Interfaces;
//using EasyBooking.Data.Entities;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace EasyBooking.API.Controllers.Admin
//{
//    [Route("api/admin/[controller]")]
//    [ApiController]
//    [Authorize(Roles = "Admin")]
//    public class UserController : ControllerBase
//    {
//        private readonly IUserService _userService;
//        public UserController(IUserService userService)
//        {
//            _userService = userService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var users = await _userService.GetAllAsync();
//            return Ok(users);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var user = await _userService.GetByIdAsync(id);
//            if (user == null) return NotFound();
//            return Ok(user);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] User user)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            await _userService.AddAsync(user);
//            return Ok();
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, [FromBody] User user)
//        {
//            if (id != user.Id) return BadRequest();
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            await _userService.UpdateAsync(user);
//            return Ok();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            await _userService.DeleteAsync(id);
//            return Ok();
//        }
//    }
//} 