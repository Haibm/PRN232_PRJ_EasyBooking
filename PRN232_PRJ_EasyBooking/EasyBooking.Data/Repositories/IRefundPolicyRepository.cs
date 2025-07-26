using EasyBooking.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyBooking.Data.Repositories
{
    public interface IRefundPolicyRepository
    {
        Task<IEnumerable<RefundPolicy>> GetAllAsync();
        Task<RefundPolicy> GetByIdAsync(int id);
        Task<RefundPolicy> GetActivePolicyAsync();
        Task AddAsync(RefundPolicy refundPolicy);
        Task UpdateAsync(RefundPolicy refundPolicy);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task DeactivateAllPoliciesAsync();
        Task ActivatePolicyAsync(int policyId);
    }
} 