using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Repositories;
using EasyBooking.Data.Entities;

namespace EasyBooking.Business.Services
{
    public class PointConfigService : IPointConfigService
    {
        private readonly IPointConfigRepository _pointConfigRepository;

        public PointConfigService(IPointConfigRepository pointConfigRepository)
        {
            _pointConfigRepository = pointConfigRepository;
        }

        public async Task<IEnumerable<PointConfigDto>> GetAllAsync()
        {
            Console.WriteLine("PointConfigService.GetAllAsync called");
            var entities = await _pointConfigRepository.GetAllAsync();
            Console.WriteLine($"PointConfigService.GetAllAsync returned {entities.Count()} entities");
            
            var result = entities.Select(MapToDto).Where(dto => dto != null).ToList();
            Console.WriteLine($"PointConfigService.GetAllAsync mapped {result.Count} DTOs");
            
            return result;
        }

        public async Task<PointConfigDto> GetByIdAsync(int id)
        {
            var entity = await _pointConfigRepository.GetByIdAsync(id);
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<PointConfigDto> GetActiveConfigAsync()
        {
            var entity = await _pointConfigRepository.GetActiveConfigAsync();
            return entity != null ? MapToDto(entity) : null;
        }

        public async Task<PointConfigDto> AddAsync(PointConfigDto pointConfigDto)
        {
            var entity = MapToEntity(pointConfigDto);
            
            // Đảm bảo audit fields được set đúng
            if (string.IsNullOrEmpty(entity.CreateBy))
            {
                entity.CreateBy = "Staff";
            }
            if (entity.CreateAt == default)
            {
                entity.CreateAt = DateTime.Now;
            }
            
            var result = await _pointConfigRepository.AddAsync(entity);
            return MapToDto(result);
        }

        public async Task<PointConfigDto> UpdateAsync(PointConfigDto pointConfigDto)
        {
            var entity = MapToEntity(pointConfigDto);
            
            // Đảm bảo audit fields được set đúng
            if (string.IsNullOrEmpty(entity.UpdateBy))
            {
                entity.UpdateBy = "Staff";
            }
            entity.UpdateAt = DateTime.Now;
            
            var result = await _pointConfigRepository.UpdateAsync(entity);
            return MapToDto(result);
        }

        public async Task DeleteAsync(int id)
        {
            // Lấy thông tin config trước khi xóa để set DeleteBy
            var config = await _pointConfigRepository.GetByIdAsync(id);
            if (config != null)
            {
                config.DeleteBy = "Staff"; // Có thể cải thiện bằng cách lấy từ JWT token
                config.DeleteAt = DateTime.Now;
                await _pointConfigRepository.UpdateAsync(config);
            }
            else
            {
                await _pointConfigRepository.DeleteAsync(id);
            }
        }

        public async Task DeleteAsync(int id, string username)
        {
            // Lấy thông tin config trước khi xóa để set DeleteBy
            var config = await _pointConfigRepository.GetByIdAsync(id);
            if (config != null)
            {
                config.DeleteBy = username;
                config.DeleteAt = DateTime.Now;
                await _pointConfigRepository.UpdateAsync(config);
            }
            else
            {
                await _pointConfigRepository.DeleteAsync(id);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _pointConfigRepository.ExistsAsync(id);
        }

        public async Task<int> CalculatePointsAsync(decimal amount)
        {
            var activeConfig = await GetActiveConfigAsync();
            if (activeConfig == null)
            {
                // Fallback to default: 10 points per 100,000 VND
                return (int)(amount / 100000 * 10);
            }

            if (amount < activeConfig.MinAmountToEarn)
            {
                return 0;
            }

            return (int)(amount / activeConfig.AmountPerPoint);
        }

        private PointConfigDto MapToDto(PointConfig entity)
        {
            if (entity == null)
            {
                Console.WriteLine("MapToDto: entity is null");
                return null;
            }
            
            Console.WriteLine($"Mapping entity: PointConfigId={entity.PointConfigId}, ConfigName={entity.ConfigName}");
            
            try
            {
                var dto = new PointConfigDto
                {
                    PointConfigId = entity.PointConfigId,
                    ConfigName = entity.ConfigName ?? "",
                    Description = entity.Description ?? "",
                    AmountPerPoint = entity.AmountPerPoint,
                    MinAmountToEarn = entity.MinAmountToEarn,
                    IsActive = entity.IsActive,
                    CreateAt = entity.CreateAt,
                    CreateBy = entity.CreateBy ?? "",
                    UpdateAt = entity.UpdateAt,
                    UpdateBy = entity.UpdateBy ?? "",
                    DeleteAt = entity.DeleteAt,
                    DeleteBy = entity.DeleteBy ?? "",
                    IsDelete = entity.IsDelete
                };
                
                Console.WriteLine($"Mapped DTO: PointConfigId={dto.PointConfigId}, ConfigName={dto.ConfigName}");
                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MapToDto error: {ex.Message}");
                Console.WriteLine($"MapToDto stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private PointConfig MapToEntity(PointConfigDto dto)
        {
            return new PointConfig
            {
                PointConfigId = dto.PointConfigId,
                ConfigName = dto.ConfigName,
                Description = dto.Description,
                AmountPerPoint = dto.AmountPerPoint,
                MinAmountToEarn = dto.MinAmountToEarn,
                IsActive = dto.IsActive,
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