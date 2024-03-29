using System.Collections.Generic;
using System.Threading.Tasks;
using Calcpad.web.Data.Models;

namespace Calcpad.web.Data.Services
{
    /// <summary>
    /// Interface for the Order Service.
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Adds a new order.
        /// </summary>
        /// <param name="order">The order to add.</param>
        /// <returns>The added order.</returns>
        Task<Order> AddAsync(Order order);

        /// <summary>
        /// Gets an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order.</param>
        /// <returns>The order with the given ID.</returns>
        Task<Order> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="order">The updated order.</param>
        /// <returns>The updated order.</returns>
        Task<Order> UpdateAsync(Order order);

        /// <summary>
        /// Deletes an order by its ID.
        /// </summary>
        /// <param name="id">The ID of the order to delete.</param>
        /// <returns>The deleted order.</returns>
        Task<Order> DeleteAsync(int id);

        /// <summary>
        /// Gets all orders by a user's ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of orders by the given user.</returns>
        Task<List<Order>> GetByUserIdAsync(string userId);

        /// <summary>
        /// Gets the last order by a user's ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>The last order by the given user.</returns>
        Task<Order> GetLastByUserIdAsync(string userId);

        /// <summary>
        /// Gets all orders that are set to expire.
        /// </summary>
        /// <returns>A list of orders that are set to expire.</returns>
        Task<List<Order>> GetOrdersToExpireAsync();

        /// <summary>
        /// Gets all orders that are set to start.
        /// </summary>
        /// <returns>A list of orders that are set to start.</returns>
        Task<List<Order>> GetOrdersToStartAsync();
    }
}