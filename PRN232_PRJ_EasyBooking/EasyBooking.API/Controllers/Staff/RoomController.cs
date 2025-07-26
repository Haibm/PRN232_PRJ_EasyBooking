using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace EasyBooking.API.Controllers.Staff
{
    [Route("api/staff/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllAsync();
            // Lọc chỉ lấy phòng chưa bị xóa
            var activeRooms = rooms.Where(r => r.IsDelete != true);
            return Ok(activeRooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomDto roomDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Gán thông tin tạo
            var username = User.Identity?.Name ?? "system";
            roomDto.CreateBy = username;
            roomDto.CreateAt = DateTime.Now;
            roomDto.IsDelete = false;
            await _roomService.AddAsync(roomDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoomDto roomDto)
        {
            if (id != roomDto.RoomId) return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Gán thông tin cập nhật
            var username = User.Identity?.Name ?? "system";
            roomDto.UpdateBy = username;
            roomDto.UpdateAt = DateTime.Now;
            await _roomService.UpdateAsync(roomDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Soft delete
            var room = await _roomService.GetByIdAsync(id);
            if (room == null) return NotFound();
            var username = User.Identity?.Name ?? "system";
            room.IsDelete = true;
            room.DeleteBy = username;
            room.DeleteAt = DateTime.Now;
            room.UpdateBy = username;
            room.UpdateAt = DateTime.Now;
            await _roomService.UpdateAsync(room);
            return NoContent();
        }
    }
} 