using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Concurrent;

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
        public async Task AdminUpdateAsync(UserDto userDto)
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
            await _userRepository.AdminUpdateAsync(user);
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

        // Lưu code xác nhận và thông tin timeout, số lần nhập sai
        private static ConcurrentDictionary<int, (string Code, DateTime Expiry, int FailCount)> _changePasswordCodes = new();

        public async Task<string> SendChangePasswordCodeAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            // Gửi email và nhận lại mã code từ bên trong hàm SendEmail
            string code = EmailSender.SendEmail(user.Email, "Mã xác nhận đổi mật khẩu", null);

            // Lưu code vào dictionary
            _changePasswordCodes[userId] = (code, DateTime.UtcNow.AddMinutes(5), 0);

            return code;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            if (!_changePasswordCodes.TryGetValue(dto.UserId, out var info))
                return false;
            if (DateTime.UtcNow > info.Expiry)
            {
                _changePasswordCodes.TryRemove(dto.UserId, out _);
                return false;
            }
            if (dto.VerificationCode != info.Code)
            {
                // Tăng số lần nhập sai
                info.FailCount++;
                if (info.FailCount >= 5)
                {
                    _changePasswordCodes.TryRemove(dto.UserId, out _);
                }
                else
                {
                    _changePasswordCodes[dto.UserId] = (info.Code, info.Expiry, info.FailCount);
                }
                return false;
            }
            // Đúng code, kiểm tra mật khẩu cũ
            var user = await _userRepository.GetByIdAsync(dto.UserId);
            if (user == null || user.PasswordHash != dto.OldPassword)
                return false;
            if (dto.NewPassword != dto.ConfirmPassword)
                return false;
            // Đổi mật khẩu
            user.PasswordHash = dto.NewPassword;
            await _userRepository.UpdateAsync(user);
            _changePasswordCodes.TryRemove(dto.UserId, out _);
            return true;
        }
    }
} 