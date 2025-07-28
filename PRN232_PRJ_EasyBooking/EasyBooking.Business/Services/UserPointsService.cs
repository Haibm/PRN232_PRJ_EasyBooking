using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;

namespace EasyBooking.Business.Services
{
    public class UserPointsService : IUserPointsService
    {
        private readonly IUserPointsRepository _userPointsRepository;
        private readonly IPointTransactionRepository _pointTransactionRepository;

        public UserPointsService(IUserPointsRepository userPointsRepository, IPointTransactionRepository pointTransactionRepository)
        {
            _userPointsRepository = userPointsRepository;
            _pointTransactionRepository = pointTransactionRepository;
        }

        public async Task<IEnumerable<UserPointsDto>> GetAllAsync()
        {
            var entities = await _userPointsRepository.GetAllAsync();
            return entities.Select(MapToDto);
        }

        public async Task<UserPointsDto> GetByIdAsync(int id)
        {
            var entity = await _userPointsRepository.GetByIdAsync(id);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<UserPointsDto> GetByUserIdAsync(int userId)
        {
            var entity = await _userPointsRepository.GetByUserIdAsync(userId);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<UserPointsDto> AddAsync(UserPointsDto userPointsDto)
        {
            var entity = MapToEntity(userPointsDto);
            var result = await _userPointsRepository.AddAsync(entity);
            return MapToDto(result);
        }

        public async Task<UserPointsDto> UpdateAsync(UserPointsDto userPointsDto)
        {
            var entity = MapToEntity(userPointsDto);
            var result = await _userPointsRepository.UpdateAsync(entity);
            return MapToDto(result);
        }

        public async Task DeleteAsync(int id)
        {
            await _userPointsRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int userId)
        {
            return await _userPointsRepository.ExistsAsync(userId);
        }

        public async Task<int> GetUserCurrentPointsAsync(int userId)
        {
            var userPoints = await _userPointsRepository.GetByUserIdAsync(userId);
            return userPoints?.CurrentPoints ?? 0;
        }

        public async Task<int> GetUserTotalEarnedPointsAsync(int userId)
        {
            var userPoints = await _userPointsRepository.GetByUserIdAsync(userId);
            return userPoints?.TotalEarnedPoints ?? 0;
        }

        public async Task<int> GetUserTotalUsedPointsAsync(int userId)
        {
            var userPoints = await _userPointsRepository.GetByUserIdAsync(userId);
            return userPoints?.TotalUsedPoints ?? 0;
        }

        public async Task<UserPointsDto> InitializeUserPointsAsync(int userId, string createBy = "System")
        {
            var existingUserPoints = await _userPointsRepository.GetByUserIdAsync(userId);
            if (existingUserPoints != null)
            {
                return MapToDto(existingUserPoints);
            }

            var newUserPoints = new UserPoints
            {
                UserId = userId,
                CurrentPoints = 0,
                TotalEarnedPoints = 0,
                TotalUsedPoints = 0,
                CreateAt = DateTime.Now,
                CreateBy = createBy,
                IsDelete = false
            };

            var result = await _userPointsRepository.AddAsync(newUserPoints);
            return MapToDto(result);
        }

        public async Task<UserPointsDto> AddPointsAsync(int userId, int points, string description, int? orderId = null, string createBy = "System")
        {
            await InitializeUserPointsAsync(userId, createBy); // Ensure user points record exists
            var transaction = new PointTransaction { UserId = userId, Points = points, TransactionType = "EARN", Description = description, OrderId = orderId, CreateBy = createBy };
            await _pointTransactionRepository.AddAsync(transaction);
            var userPoints = await _userPointsRepository.GetByUserIdAsync(userId);
            if (userPoints != null) { userPoints.CurrentPoints += points; userPoints.TotalEarnedPoints += points; userPoints.UpdateBy = createBy; await _userPointsRepository.UpdateAsync(userPoints); }
            return MapToDto(userPoints);
        }

        public async Task<UserPointsDto> UsePointsAsync(int userId, int points, string description, int? orderId = null, string createBy = "System")
        {
            var currentPoints = await GetUserCurrentPointsAsync(userId);
            if (currentPoints < points) throw new InvalidOperationException($"Không đủ điểm. Hiện tại có {currentPoints} điểm, cần {points} điểm.");
            await InitializeUserPointsAsync(userId, createBy); // Ensure user points record exists
            var transaction = new PointTransaction { UserId = userId, Points = points, TransactionType = "USE", Description = description, OrderId = orderId, CreateBy = createBy };
            await _pointTransactionRepository.AddAsync(transaction);
            var userPoints = await _userPointsRepository.GetByUserIdAsync(userId);
            if (userPoints != null) { userPoints.CurrentPoints -= points; userPoints.TotalUsedPoints += points; userPoints.UpdateBy = createBy; await _userPointsRepository.UpdateAsync(userPoints); }
            return MapToDto(userPoints);
        }

        private UserPointsDto MapToDto(UserPoints entity)
        {
            return new UserPointsDto
            {
                UserPointsId = entity.UserPointsId,
                UserId = entity.UserId,
                CurrentPoints = entity.CurrentPoints,
                TotalEarnedPoints = entity.TotalEarnedPoints,
                TotalUsedPoints = entity.TotalUsedPoints,
                CreateAt = entity.CreateAt,
                CreateBy = entity.CreateBy,
                UpdateAt = entity.UpdateAt,
                UpdateBy = entity.UpdateBy,
                DeleteAt = entity.DeleteAt,
                DeleteBy = entity.DeleteBy,
                IsDelete = entity.IsDelete
            };
        }

        private UserPoints MapToEntity(UserPointsDto dto)
        {
            return new UserPoints
            {
                UserPointsId = dto.UserPointsId,
                UserId = dto.UserId,
                CurrentPoints = dto.CurrentPoints,
                TotalEarnedPoints = dto.TotalEarnedPoints,
                TotalUsedPoints = dto.TotalUsedPoints,
                CreateAt = dto.CreateAt,
                CreateBy = dto.CreateBy,
                UpdateAt = dto.UpdateAt,
                UpdateBy = dto.UpdateBy,
                DeleteAt = dto.DeleteAt,
                DeleteBy = dto.DeleteBy,
                IsDelete = dto.IsDelete
            };
        }
    }
} 