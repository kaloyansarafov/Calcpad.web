﻿using System.Threading.Tasks;
using Calcpad.web.Data.Models;

namespace Calcpad.web.Data.Services;

public interface IOrderService
{
    Task<Order> AddAsync(Order order);
    Task<Order> GetByIdAsync(int id);
    Task<Order> UpdateAsync(Order order);
    Task<Order> DeleteAsync(int id);
}