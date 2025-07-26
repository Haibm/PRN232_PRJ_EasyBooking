using EasyBooking.Business.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Business.Interfaces
{
    public interface IRefundPolicyService
    {
        Task<IEnumerable<RefundPolicyDto>> GetAllAsync();
        Task<RefundPolicyDto> GetByIdAsync(int id);
        Task<RefundPolicyDto> GetActivePolicyAsync();
        Task AddAsync(RefundPolicyDto refundPolicyDto);
        Task UpdateAsync(RefundPolicyDto refundPolicyDto);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<decimal> GetCurrentRefundPercentageAsync();
        Task ActivatePolicyAsync(int policyId);
    }
} 