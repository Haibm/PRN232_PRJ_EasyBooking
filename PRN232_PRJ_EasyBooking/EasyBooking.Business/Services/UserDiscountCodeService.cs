using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;

namespace EasyBooking.Business.Services
{
    public class UserDiscountCodeService : IUserDiscountCodeService
    {
        private readonly IUserDiscountCodeRepository _userDiscountCodeRepository;
        private readonly IDiscountCodeRepository _discountCodeRepository;
        private readonly IUserPointsService _userPointsService;

        public UserDiscountCodeService(
            IUserDiscountCodeRepository userDiscountCodeRepository,
            IDiscountCodeRepository discountCodeRepository,
            IUserPointsService userPointsService)
        {
            _userDiscountCodeRepository = userDiscountCodeRepository;
            _discountCodeRepository = discountCodeRepository;
            _userPointsService = userPointsService;
        }

        public async Task<IEnumerable<UserDiscountCodeDto>> GetAllAsync()
        {
            var entities = await _userDiscountCodeRepository.GetAllAsync();
            return entities.Select(MapToDto);
        }

        public async Task<UserDiscountCodeDto> GetByIdAsync(int id)
        {
            var entity = await _userDiscountCodeRepository.GetByIdAsync(id);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<IEnumerable<UserDiscountCodeDto>> GetByUserIdAsync(int userId)
        {
            var entities = await _userDiscountCodeRepository.GetByUserIdAsync(userId);
            return entities.Select(MapToDto);
        }

        public async Task<IEnumerable<UserDiscountCodeDto>> GetByDiscountCodeIdAsync(int discountCodeId)
        {
            var entities = await _userDiscountCodeRepository.GetByDiscountCodeIdAsync(discountCodeId);
            return entities.Select(MapToDto);
        }

        public async Task<UserDiscountCodeDto> GetByUserAndCodeAsync(int userId, int discountCodeId)
        {
            var entity = await _userDiscountCodeRepository.GetByUserAndCodeAsync(userId, discountCodeId);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<IEnumerable<UserDiscountCodeDto>> GetUnusedByUserIdAsync(int userId)
        {
            var entities = await _userDiscountCodeRepository.GetUnusedByUserIdAsync(userId);
            return entities.Select(MapToDto);
        }

        public async Task<UserDiscountCodeDto> AddAsync(UserDiscountCodeDto userDiscountCodeDto)
        {
            var entity = MapToEntity(userDiscountCodeDto);
            var result = await _userDiscountCodeRepository.AddAsync(entity);
            return MapToDto(result);
        }

        public async Task<UserDiscountCodeDto> UpdateAsync(UserDiscountCodeDto userDiscountCodeDto)
        {
            var entity = MapToEntity(userDiscountCodeDto);
            var result = await _userDiscountCodeRepository.UpdateAsync(entity);
            return MapToDto(result);
        }

        public async Task DeleteAsync(int id)
        {
            await _userDiscountCodeRepository.DeleteAsync(id);
        }

        public async Task<bool> HasUserRedeemedCodeAsync(int userId, int discountCodeId)
        {
            return await _userDiscountCodeRepository.HasUserRedeemedCodeAsync(userId, discountCodeId);
        }

        public async Task<int> GetUserRedeemedCountAsync(int userId, int discountCodeId)
        {
            return await _userDiscountCodeRepository.GetUserRedeemedCountAsync(userId, discountCodeId);
        }

        public async Task<UserDiscountCodeDto> RedeemCodeAsync(int userId, int discountCodeId, int pointsUsed, string createBy = "System")
        {
            if (await HasUserRedeemedCodeAsync(userId, discountCodeId)) throw new InvalidOperationException("Bạn đã đổi mã giảm giá này rồi.");
            var discountCode = await _discountCodeRepository.GetByIdAsync(discountCodeId);
            if (discountCode == null || !discountCode.IsActive || discountCode.StartDate > DateTime.Now || discountCode.EndDate < DateTime.Now || (discountCode.MaxUsage > 0 && discountCode.CurrentUsage >= discountCode.MaxUsage)) throw new InvalidOperationException("Mã giảm giá không tồn tại hoặc không hợp lệ.");
            if (discountCode.RequiredPoints > 0)
            {
                var currentPoints = await _userPointsService.GetUserCurrentPointsAsync(userId);
                if (currentPoints < discountCode.RequiredPoints) throw new InvalidOperationException($"Không đủ điểm. Cần {discountCode.RequiredPoints} điểm, hiện tại có {currentPoints} điểm.");
                await _userPointsService.UsePointsAsync(userId, discountCode.RequiredPoints, $"Đổi mã giảm giá: {discountCode.Code}", null, createBy);
            }
            var userDiscountCode = new UserDiscountCode { UserId = userId, DiscountCodeId = discountCodeId, PointsUsed = discountCode.RequiredPoints, RedeemDate = DateTime.Now, IsUsed = false, CreateBy = createBy };
            var result = await _userDiscountCodeRepository.AddAsync(userDiscountCode);
            return MapToDto(result);
        }

        public async Task<bool> UseCodeAsync(int userDiscountCodeId, int orderId, string updateBy = "System")
        {
            var userDiscountCode = await _userDiscountCodeRepository.GetByIdAsync(userDiscountCodeId);
            if (userDiscountCode == null || userDiscountCode.IsUsed) return false;
            userDiscountCode.IsUsed = true;
            userDiscountCode.UsedDate = DateTime.Now;
            userDiscountCode.OrderId = orderId;
            userDiscountCode.UpdateAt = DateTime.Now;
            userDiscountCode.UpdateBy = updateBy;
            await _userDiscountCodeRepository.UpdateAsync(userDiscountCode);
            return true;
        }

        private UserDiscountCodeDto MapToDto(UserDiscountCode entity)
        {
            return new UserDiscountCodeDto
            {
                UserDiscountCodeId = entity.UserDiscountCodeId,
                UserId = entity.UserId,
                DiscountCodeId = entity.DiscountCodeId,
                PointsUsed = entity.PointsUsed,
                RedeemDate = entity.RedeemDate,
                IsUsed = entity.IsUsed,
                UsedDate = entity.UsedDate,
                OrderId = entity.OrderId,
                CreateAt = entity.CreateAt,
                CreateBy = entity.CreateBy,
                UpdateAt = entity.UpdateAt,
                UpdateBy = entity.UpdateBy,
                DeleteAt = entity.DeleteAt,
                DeleteBy = entity.DeleteBy,
                IsDelete = entity.IsDelete,
                DiscountCodeName = entity.DiscountCode?.Name,
                DiscountCodeCode = entity.DiscountCode?.Code,
                DiscountPercentage = entity.DiscountCode?.DiscountPercentage ?? 0,
                DiscountAmount = entity.DiscountCode?.DiscountAmount ?? 0
            };
        }

        private UserDiscountCode MapToEntity(UserDiscountCodeDto dto)
        {
            return new UserDiscountCode
            {
                UserDiscountCodeId = dto.UserDiscountCodeId,
                UserId = dto.UserId,
                DiscountCodeId = dto.DiscountCodeId,
                PointsUsed = dto.PointsUsed,
                RedeemDate = dto.RedeemDate,
                IsUsed = dto.IsUsed,
                UsedDate = dto.UsedDate,
                OrderId = dto.OrderId,
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