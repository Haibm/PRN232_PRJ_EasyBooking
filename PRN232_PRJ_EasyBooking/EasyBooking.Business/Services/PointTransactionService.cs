using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Repositories;
using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Services
{
    public class PointTransactionService : IPointTransactionService
    {
        private readonly IPointTransactionRepository _pointTransactionRepository;

        public PointTransactionService(IPointTransactionRepository pointTransactionRepository)
        {
            _pointTransactionRepository = pointTransactionRepository;
        }

        public async Task<IEnumerable<PointTransactionDto>> GetAllAsync()
        {
            var entities = await _pointTransactionRepository.GetAllAsync();
            return entities.Select(MapToDto);
        }

        public async Task<PointTransactionDto> GetByIdAsync(int id)
        {
            var entity = await _pointTransactionRepository.GetByIdAsync(id);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<IEnumerable<PointTransactionDto>> GetByUserIdAsync(int userId)
        {
            var entities = await _pointTransactionRepository.GetByUserIdAsync(userId);
            return entities.Select(MapToDto);
        }

        public async Task<IEnumerable<PointTransactionDto>> GetByUserIdAndTypeAsync(int userId, string transactionType)
        {
            var entities = await _pointTransactionRepository.GetByUserIdAndTypeAsync(userId, transactionType);
            return entities.Select(MapToDto);
        }

        public async Task<IEnumerable<PointTransactionDto>> GetByOrderIdAsync(int orderId)
        {
            var entities = await _pointTransactionRepository.GetByOrderIdAsync(orderId);
            return entities.Select(MapToDto);
        }

        public async Task<IEnumerable<PointTransactionDto>> GetByDiscountCodeIdAsync(int discountCodeId)
        {
            var entities = await _pointTransactionRepository.GetByDiscountCodeIdAsync(discountCodeId);
            return entities.Select(MapToDto);
        }

        public async Task<PointTransactionDto> AddAsync(PointTransactionDto pointTransactionDto)
        {
            var entity = MapToEntity(pointTransactionDto);
            var result = await _pointTransactionRepository.AddAsync(entity);
            return MapToDto(result);
        }

        public async Task<PointTransactionDto> UpdateAsync(PointTransactionDto pointTransactionDto)
        {
            var entity = MapToEntity(pointTransactionDto);
            var result = await _pointTransactionRepository.UpdateAsync(entity);
            return MapToDto(result);
        }

        public async Task DeleteAsync(int id)
        {
            await _pointTransactionRepository.DeleteAsync(id);
        }

        public async Task<int> GetUserTotalPointsAsync(int userId)
        {
            var transactions = await GetByUserIdAsync(userId);
            return transactions.Sum(t => t.TransactionType == "Earn" ? t.Points : -t.Points);
        }

        public async Task<int> GetUserEarnedPointsAsync(int userId)
        {
            var transactions = await GetByUserIdAndTypeAsync(userId, "Earn");
            return transactions.Sum(t => t.Points);
        }

        public async Task<int> GetUserUsedPointsAsync(int userId)
        {
            var transactions = await GetByUserIdAndTypeAsync(userId, "Use");
            return transactions.Sum(t => t.Points);
        }

        public async Task<PointTransactionDto> CreateEarnTransactionAsync(int userId, int points, string description, int? orderId = null, string createBy = "System")
        {
            var transaction = new PointTransactionDto
            {
                UserId = userId,
                Points = points,
                TransactionType = "Earn",
                Description = description,
                OrderId = orderId,
                TransactionDate = DateTime.Now,
                CreateBy = createBy
            };

            return await AddAsync(transaction);
        }

        public async Task<PointTransactionDto> CreateUseTransactionAsync(int userId, int points, string description, int? orderId = null, string createBy = "System")
        {
            var transaction = new PointTransactionDto
            {
                UserId = userId,
                Points = points,
                TransactionType = "Use",
                Description = description,
                OrderId = orderId,
                TransactionDate = DateTime.Now,
                CreateBy = createBy
            };

            return await AddAsync(transaction);
        }

        public async Task<PointTransactionDto> CreateRedeemTransactionAsync(int userId, int points, string description, int discountCodeId, string createBy = "System")
        {
            var transaction = new PointTransactionDto
            {
                UserId = userId,
                Points = points,
                TransactionType = "Redeem",
                Description = description,
                DiscountCodeId = discountCodeId,
                TransactionDate = DateTime.Now,
                CreateBy = createBy
            };

            return await AddAsync(transaction);
        }

        private PointTransactionDto MapToDto(PointTransaction entity)
        {
            return new PointTransactionDto
            {
                PointTransactionId = entity.PointTransactionId,
                UserId = entity.UserId,
                Points = entity.Points,
                TransactionType = entity.TransactionType,
                Description = entity.Description,
                OrderId = entity.OrderId,
                DiscountCodeId = entity.DiscountCodeId,
                TransactionDate = entity.TransactionDate,
                CreateBy = entity.CreateBy,
                CreateAt = entity.CreateAt,
                UpdateBy = entity.UpdateBy,
                UpdateAt = entity.UpdateAt,
                IsDelete = entity.IsDelete,
                DeleteAt = entity.DeleteAt,
                TransactionTypeDisplay = GetTransactionTypeDisplay(entity.TransactionType)
            };
        }

        private PointTransaction MapToEntity(PointTransactionDto dto)
        {
            return new PointTransaction
            {
                PointTransactionId = dto.PointTransactionId,
                UserId = dto.UserId,
                Points = dto.Points,
                TransactionType = dto.TransactionType,
                Description = dto.Description,
                OrderId = dto.OrderId,
                DiscountCodeId = dto.DiscountCodeId,
                TransactionDate = dto.TransactionDate,
                CreateBy = dto.CreateBy,
                CreateAt = dto.CreateAt,
                UpdateBy = dto.UpdateBy,
                UpdateAt = dto.UpdateAt,
                IsDelete = dto.IsDelete,
                DeleteAt = dto.DeleteAt
            };
        }

        private string GetTransactionTypeDisplay(string transactionType)
        {
            return transactionType switch
            {
                "Earn" => "Tích điểm",
                "Use" => "Sử dụng điểm",
                "Redeem" => "Đổi mã giảm giá",
                _ => transactionType
            };
        }
    }
} 