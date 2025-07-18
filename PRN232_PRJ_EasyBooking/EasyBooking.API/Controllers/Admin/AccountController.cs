﻿using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyBooking.API.Controllers.Admin
{
    [Route("api/admin/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _userService.AddAsync(userDto);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.UserId) return BadRequest(new { error = "Id trên URL không khớp với UserId trong dữ liệu gửi lên." });
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _userService.AdminUpdateAsync(userDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
