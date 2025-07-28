using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Repositories;
using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Services
{
    public class DiscountCodeService : IDiscountCodeService
    {
        private readonly IDiscountCodeRepository _discountCodeRepository;
        private readonly IUserDiscountCodeRepository _userDiscountCodeRepository;

        public DiscountCodeService(IDiscountCodeRepository discountCodeRepository, IUserDiscountCodeRepository userDiscountCodeRepository)
        {
            _discountCodeRepository = discountCodeRepository;
            _userDiscountCodeRepository = userDiscountCodeRepository;
        }

        public async Task<IEnumerable<DiscountCodeDto>> GetAllAsync()
        {
            var entities = await _discountCodeRepository.GetAllAsync();
            return entities.Select(MapToDto);
        }

        public async Task<DiscountCodeDto> GetByIdAsync(int id)
        {
            var entity = await _discountCodeRepository.GetByIdAsync(id);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<DiscountCodeDto> GetByCodeAsync(string code)
        {
            var entity = await _discountCodeRepository.GetByCodeAsync(code);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<IEnumerable<DiscountCodeDto>> GetActiveCodesAsync()
        {
            var entities = await _discountCodeRepository.GetActiveCodesAsync();
            return entities.Select(MapToDto);
        }

        public async Task<IEnumerable<DiscountCodeDto>> GetAvailableCodesAsync()
        {
            var entities = await _discountCodeRepository.GetAvailableCodesAsync();
            return entities.Select(MapToDto);
        }

        public async Task<DiscountCodeDto> AddAsync(DiscountCodeDto discountCodeDto)
        {
            var entity = MapToEntity(discountCodeDto);
            var result = await _discountCodeRepository.AddAsync(entity);
            return MapToDto(result);
        }

        public async Task<DiscountCodeDto> UpdateAsync(DiscountCodeDto discountCodeDto)
        {
            var entity = MapToEntity(discountCodeDto);
            var result = await _discountCodeRepository.UpdateAsync(entity);
            return MapToDto(result);
        }

        public async Task DeleteAsync(int id)
        {
            await _discountCodeRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(string code)
        {
            return await _discountCodeRepository.ExistsAsync(code);
        }

        public async Task<bool> IsCodeValidAsync(string code)
        {
            var discountCode = await GetByCodeAsync(code);
            if (discountCode == null) return false;

            var now = DateTime.Now;
            return discountCode.IsActive && 
                   discountCode.StartDate <= now && 
                   discountCode.EndDate >= now &&
                   discountCode.CurrentUsage < discountCode.MaxUsage;
        }

        public async Task<bool> IsCodeAvailableForUserAsync(string code, int userId)
        {
            if (!await IsCodeValidAsync(code)) return false;

            var discountCode = await GetByCodeAsync(code);
            if (discountCode == null) return false;

            // Check if user has already used this code
            var userUsage = await _userDiscountCodeRepository.GetByUserAndCodeAsync(userId, discountCode.DiscountCodeId);
            if (userUsage != null) return false;

            // Check if user has enough points (if required)
            if (discountCode.RequiredPoints > 0)
            {
                // This would need IUserPointsService injection - simplified for now
                return true; // Assume user has enough points
            }

            return true;
        }

        public async Task<decimal> CalculateDiscountAsync(string code, decimal originalAmount)
        {
            var discountCode = await GetByCodeAsync(code);
            if (discountCode == null || !await IsCodeValidAsync(code)) return 0;

            // Tính discount dựa trên percentage hoặc amount
            decimal discountAmount = 0;
            if (discountCode.DiscountPercentage > 0)
            {
                discountAmount = originalAmount * (discountCode.DiscountPercentage / 100m);
            }
            else
            {
                discountAmount = discountCode.DiscountAmount;
            }

            return Math.Min(discountAmount, originalAmount);
        }

        public async Task<bool> UseCodeAsync(string code, int userId, int orderId)
        {
            if (!await IsCodeAvailableForUserAsync(code, userId)) return false;

            var discountCode = await GetByCodeAsync(code);
            if (discountCode == null) return false;

            // Update usage count
            discountCode.CurrentUsage++;
            await UpdateAsync(discountCode);

            // Record user usage
            var userDiscountCode = new UserDiscountCodeDto
            {
                UserId = userId,
                DiscountCodeId = discountCode.DiscountCodeId,
                OrderId = orderId,
                RedeemDate = DateTime.Now,
                CreateBy = "System"
            };

            await _userDiscountCodeRepository.AddAsync(MapToEntity(userDiscountCode));
            return true;
        }

        private DiscountCodeDto MapToDto(DiscountCode entity)
        {
            return new DiscountCodeDto
            {
                DiscountCodeId = entity.DiscountCodeId,
                Code = entity.Code,
                Name = entity.Name,
                Description = entity.Description,
                DiscountPercentage = entity.DiscountPercentage,
                DiscountAmount = entity.DiscountAmount,
                RequiredPoints = entity.RequiredPoints,
                MaxUsage = entity.MaxUsage,
                CurrentUsage = entity.CurrentUsage,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                IsActive = entity.IsActive,
                CreateBy = entity.CreateBy,
                CreateAt = entity.CreateAt,
                UpdateBy = entity.UpdateBy,
                UpdateAt = entity.UpdateAt,
                IsDelete = entity.IsDelete,
                DeleteAt = entity.DeleteAt
            };
        }

        private DiscountCode MapToEntity(DiscountCodeDto dto)
        {
            return new DiscountCode
            {
                DiscountCodeId = dto.DiscountCodeId,
                Code = dto.Code,
                Name = dto.Name,
                Description = dto.Description,
                DiscountPercentage = dto.DiscountPercentage,
                DiscountAmount = dto.DiscountAmount,
                RequiredPoints = dto.RequiredPoints,
                MaxUsage = dto.MaxUsage,
                CurrentUsage = dto.CurrentUsage,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                IsActive = dto.IsActive,
                CreateBy = dto.CreateBy,
                CreateAt = dto.CreateAt,
                UpdateBy = dto.UpdateBy,
                UpdateAt = dto.UpdateAt,
                IsDelete = dto.IsDelete,
                DeleteAt = dto.DeleteAt
            };
        }

        private UserDiscountCode MapToEntity(UserDiscountCodeDto dto)
        {
            return new UserDiscountCode
            {
                UserDiscountCodeId = dto.UserDiscountCodeId,
                UserId = dto.UserId,
                DiscountCodeId = dto.DiscountCodeId,
                OrderId = dto.OrderId,
                RedeemDate = dto.RedeemDate,
                CreateBy = dto.CreateBy,
                CreateAt = dto.CreateAt,
                UpdateBy = dto.UpdateBy,
                UpdateAt = dto.UpdateAt,
                IsDelete = dto.IsDelete,
                DeleteAt = dto.DeleteAt
            };
        }
    }
} 