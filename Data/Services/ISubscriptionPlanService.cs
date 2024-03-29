using Calcpad.web.Data.Models;
using System.Threading.Tasks;

namespace Calcpad.web.Data.Services
{
    /// <summary>
    /// Interface for the Subscription Plan Service.
    /// </summary>
    public interface ISubscriptionPlanService
    {
        /// <summary>
        /// Adds a new subscription plan.
        /// </summary>
        /// <param name="subscriptionPlan">The subscription plan to add.</param>
        /// <returns>The added subscription plan.</returns>
        Task<SubscriptionPlan> AddAsync(SubscriptionPlan subscriptionPlan);

        /// <summary>
        /// Gets a subscription plan by its ID.
        /// </summary>
        /// <param name="id">The ID of the subscription plan.</param>
        /// <returns>The subscription plan with the given ID.</returns>
        Task<SubscriptionPlan> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing subscription plan.
        /// </summary>
        /// <param name="subscriptionPlan">The updated subscription plan.</param>
        /// <returns>The updated subscription plan.</returns>
        Task<SubscriptionPlan> UpdateAsync(SubscriptionPlan subscriptionPlan);

        /// <summary>
        /// Deletes a subscription plan by its ID.
        /// </summary>
        /// <param name="id">The ID of the subscription plan to delete.</param>
        /// <returns>The deleted subscription plan.</returns>
        Task<SubscriptionPlan> DeleteAsync(int id);
    }
}