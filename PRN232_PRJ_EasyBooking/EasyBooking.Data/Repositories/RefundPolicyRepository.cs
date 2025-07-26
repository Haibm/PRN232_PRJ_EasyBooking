using EasyBooking.Data.DbContexts;
using EasyBooking.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.Data.SqlClient;

namespace EasyBooking.Data.Repositories
{
    public class RefundPolicyRepository : IRefundPolicyRepository
    {
        private readonly CinemaBookingDbContext _context;
        public RefundPolicyRepository(CinemaBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RefundPolicy>> GetAllAsync()
        {
            // Thêm AsNoTracking để tránh EF tracking conflict
            return await _context.RefundPolicies
                .AsNoTracking()
                .Where(rp => rp.IsDelete != true)
                .ToListAsync();
        }

        public async Task<RefundPolicy> GetByIdAsync(int id)
        {
            return await _context.RefundPolicies.FindAsync(id);
        }

        public async Task<RefundPolicy> GetActivePolicyAsync()
        {
            return await _context.RefundPolicies
                .Where(rp => rp.IsActive && rp.IsDelete != true)
                .FirstOrDefaultAsync();
        }

        public async Task AddAsync(RefundPolicy refundPolicy)
        {
            await _context.RefundPolicies.AddAsync(refundPolicy);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RefundPolicy refundPolicy)
        {
            _context.RefundPolicies.Update(refundPolicy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var refundPolicy = await _context.RefundPolicies.FindAsync(id);
            if (refundPolicy != null)
            {
                refundPolicy.IsDelete = true;
                refundPolicy.DeleteAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.RefundPolicies.AnyAsync(rp => rp.RefundPolicyId == id && rp.IsDelete != true);
        }

        public async Task DeactivateAllPoliciesAsync()
        {
            var updateTime = DateTime.Now;
            var sql = "UPDATE RefundPolicy SET IsActive = 0, UpdateAt = @updateTime, UpdateBy = 'Staff' WHERE IsDelete != 1";
            await _context.Database.ExecuteSqlRawAsync(sql, new SqlParameter("@updateTime", updateTime));
        }

        public async Task ActivatePolicyAsync(int policyId)
        {
            var updateTime = DateTime.Now;
            var sql = "UPDATE RefundPolicy SET IsActive = 1, UpdateAt = @updateTime, UpdateBy = 'Staff' WHERE RefundPolicyId = @policyId AND IsDelete != 1";
            await _context.Database.ExecuteSqlRawAsync(sql, 
                new SqlParameter("@updateTime", updateTime),
                new SqlParameter("@policyId", policyId));
        }
    }
} 