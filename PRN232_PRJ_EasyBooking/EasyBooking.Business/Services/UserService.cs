using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBooking.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                PasswordHash = u.PasswordHash,
                FullName = u.FullName,
                Role = u.Role,
                Email = u.Email,
                IsActive = u.IsActive
            });
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var u = await _userRepository.GetByIdAsync(id);
            if (u == null) return null;
            return new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                PasswordHash = u.PasswordHash,
                FullName = u.FullName,
                Role = u.Role,
                Email = u.Email,
                IsActive = u.IsActive
            };
        }

        public async Task AddAsync(UserDto userDto)
        {
            var user = new User
            {
                Username = userDto.Username,
                PasswordHash = userDto.PasswordHash,
                FullName = userDto.FullName,
                Role = userDto.Role,
                Email = userDto.Email,
                IsActive = userDto.IsActive
            };
            await _userRepository.AddAsync(user);
        }

        public async Task UpdateAsync(UserDto userDto)
        {
            var user = new User
            {
                UserId = userDto.UserId,
                Username = userDto.Username,
                PasswordHash = userDto.PasswordHash,
                FullName = userDto.FullName,
                Role = userDto.Role,
                Email = userDto.Email,
                IsActive = userDto.IsActive
            };
            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public async Task<UserDto> GetByUsernamePass(string username,string pass)
        {
            var u =await _userRepository.GetByUsernamePass(username, pass);
            if (u == null) return null;
            return new UserDto
            {
                UserId = u.UserId,
                Username = u.Username,
                PasswordHash = u.PasswordHash,
                FullName = u.FullName,
                Role = u.Role,
                Email = u.Email,
                IsActive = u.IsActive
            };
        }
    }
} 