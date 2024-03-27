using System.Collections.Generic;
using System.Threading.Tasks;
using Calcpad.web.Data.Models;

namespace Calcpad.web.Data.Services;

public interface IOrderService
{
    Task<Order> AddAsync(Order order);
    Task<Order> GetByIdAsync(int id);
    Task<Order> UpdateAsync(Order order);
    Task<Order> DeleteAsync(int id);
    Task<List<Order>> GetByUserIdAsync(string userId);
    Task<Order> GetLastByUserIdAsync(string userId);
    Task<List<Order>> GetOrdersToExpireAsync();
    Task<List<Order>> GetOrdersToStartAsync();
}