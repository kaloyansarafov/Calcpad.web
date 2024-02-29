using Calcpad.web.Data.Models;
using System.Threading.Tasks;

namespace Calcpad.web.Data.Services
{
    public interface ISubscriptionPlanService
    {
        Task<SubscriptionPlan> AddAsync(SubscriptionPlan subscriptionPlan);
        Task<SubscriptionPlan> GetByIdAsync(int id);
        Task<SubscriptionPlan> UpdateAsync(SubscriptionPlan subscriptionPlan);
        Task<SubscriptionPlan> DeleteAsync(int id);
    }
}