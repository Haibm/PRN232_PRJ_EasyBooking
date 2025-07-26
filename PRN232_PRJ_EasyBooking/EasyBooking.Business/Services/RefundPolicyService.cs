using EasyBooking.Business.DTOs;
using EasyBooking.Business.Interfaces;
using EasyBooking.Data.Entities;
using EasyBooking.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyBooking.Business.Services
{
    public class RefundPolicyService : IRefundPolicyService
    {
        private readonly IRefundPolicyRepository _refundPolicyRepository;
        public RefundPolicyService(IRefundPolicyRepository refundPolicyRepository)
        {
            _refundPolicyRepository = refundPolicyRepository;
        }

        public async Task<IEnumerable<RefundPolicyDto>> GetAllAsync()
        {
            var refundPolicies = await _refundPolicyRepository.GetAllAsync();
            return refundPolicies.Select(rp => new RefundPolicyDto
            {
                RefundPolicyId = rp.RefundPolicyId,
                PolicyName = rp.PolicyName,
                RefundPercentage = rp.RefundPercentage,
                Description = rp.Description,
                IsActive = rp.IsActive,
                CreateAt = rp.CreateAt,
                CreateBy = rp.CreateBy,
                UpdateAt = rp.UpdateAt,
                UpdateBy = rp.UpdateBy,
                DeleteAt = rp.DeleteAt,
                DeleteBy = rp.DeleteBy,
                IsDelete = rp.IsDelete
            });
        }

        public async Task<RefundPolicyDto> GetByIdAsync(int id)
        {
            var rp = await _refundPolicyRepository.GetByIdAsync(id);
            if (rp == null) return null;
            return new RefundPolicyDto
            {
                RefundPolicyId = rp.RefundPolicyId,
                PolicyName = rp.PolicyName,
                RefundPercentage = rp.RefundPercentage,
                Description = rp.Description,
                IsActive = rp.IsActive,
                CreateAt = rp.CreateAt,
                CreateBy = rp.CreateBy,
                UpdateAt = rp.UpdateAt,
                UpdateBy = rp.UpdateBy,
                DeleteAt = rp.DeleteAt,
                DeleteBy = rp.DeleteBy,
                IsDelete = rp.IsDelete
            };
        }

        public async Task<RefundPolicyDto> GetActivePolicyAsync()
        {
            var rp = await _refundPolicyRepository.GetActivePolicyAsync();
            if (rp == null) return null;
            return new RefundPolicyDto
            {
                RefundPolicyId = rp.RefundPolicyId,
                PolicyName = rp.PolicyName,
                RefundPercentage = rp.RefundPercentage,
                Description = rp.Description,
                IsActive = rp.IsActive,
                CreateAt = rp.CreateAt,
                CreateBy = rp.CreateBy,
                UpdateAt = rp.UpdateAt,
                UpdateBy = rp.UpdateBy,
                DeleteAt = rp.DeleteAt,
                DeleteBy = rp.DeleteBy,
                IsDelete = rp.IsDelete
            };
        }

        public async Task AddAsync(RefundPolicyDto refundPolicyDto)
        {
            var refundPolicy = new RefundPolicy
            {
                PolicyName = refundPolicyDto.PolicyName,
                RefundPercentage = refundPolicyDto.RefundPercentage,
                Description = refundPolicyDto.Description,
                IsActive = refundPolicyDto.IsActive,
                CreateAt = refundPolicyDto.CreateAt,
                CreateBy = refundPolicyDto.CreateBy,
                UpdateAt = refundPolicyDto.UpdateAt,
                UpdateBy = refundPolicyDto.UpdateBy,
                DeleteAt = refundPolicyDto.DeleteAt,
                DeleteBy = refundPolicyDto.DeleteBy,
                IsDelete = refundPolicyDto.IsDelete
            };
            await _refundPolicyRepository.AddAsync(refundPolicy);
        }

        public async Task UpdateAsync(RefundPolicyDto refundPolicyDto)
        {
            // Lấy entity từ DB để tránh tracking conflict
            var existingEntity = await _refundPolicyRepository.GetByIdAsync(refundPolicyDto.RefundPolicyId);
            if (existingEntity == null)
                throw new InvalidOperationException($"Policy with ID {refundPolicyDto.RefundPolicyId} not found");
            
            // Update các trường từ DTO
            existingEntity.PolicyName = refundPolicyDto.PolicyName;
            existingEntity.RefundPercentage = refundPolicyDto.RefundPercentage;
            existingEntity.Description = refundPolicyDto.Description;
            existingEntity.IsActive = refundPolicyDto.IsActive;
            existingEntity.UpdateAt = refundPolicyDto.UpdateAt;
            existingEntity.UpdateBy = refundPolicyDto.UpdateBy;
            
            await _refundPolicyRepository.UpdateAsync(existingEntity);
        }

        public async Task DeleteAsync(int id)
        {
            await _refundPolicyRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _refundPolicyRepository.ExistsAsync(id);
        }

        public async Task<decimal> GetCurrentRefundPercentageAsync()
        {
            var activePolicy = await _refundPolicyRepository.GetActivePolicyAsync();
            return activePolicy?.RefundPercentage ?? 0.8m; // Default 80%
        }

        public async Task ActivatePolicyAsync(int policyId)
        {
            // Tắt tất cả policy khác trước
            await _refundPolicyRepository.DeactivateAllPoliciesAsync();
            
            // Kích hoạt policy được chọn
            await _refundPolicyRepository.ActivatePolicyAsync(policyId);
        }
    }
} 