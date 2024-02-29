using Calcpad.web.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Calcpad.web.Data.Services
{
    public class SubscriptionPlanService : ISubscriptionPlanService
    {
        private readonly ApplicationDbContext _context;
        public SubscriptionPlanService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SubscriptionPlan> AddAsync(SubscriptionPlan subscriptionPlan)
        {
            await _context.SubscriptionPlans.AddAsync(subscriptionPlan);
            await _context.SaveChangesAsync();
            return subscriptionPlan;
        }

        public async Task<SubscriptionPlan> GetByIdAsync(int id)
        {
            if (id == 0)
                return await _context.SubscriptionPlans.FirstOrDefaultAsync();

            return await _context.SubscriptionPlans
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<SubscriptionPlan> UpdateAsync(SubscriptionPlan subscriptionPlan)
        {
            EntityEntry subscriptionPlanEntry = _context.SubscriptionPlans.Attach(subscriptionPlan);
            subscriptionPlanEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return subscriptionPlan;
        }

        public async Task<SubscriptionPlan> DeleteAsync(int id)
        {
            SubscriptionPlan subscriptionPlan = await _context.SubscriptionPlans.FindAsync(id);
            if (subscriptionPlan != null)
            {
                _context.SubscriptionPlans.Remove(subscriptionPlan);
                await _context.SaveChangesAsync();
            }
            return subscriptionPlan;
        }
    }
}