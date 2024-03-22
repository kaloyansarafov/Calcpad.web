using Calcpad.web.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading.Tasks;

namespace Calcpad.web.Data.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            if (id == 0)
                return await _context.Orders.FirstOrDefaultAsync();

            return await _context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Include(o => o.Plan)
                .Include(o => o.Invoice)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            EntityEntry orderEntry = _context.Orders.Attach(order);
            orderEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> DeleteAsync(int id)
        {
            Order order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return order;
        }
    }
}