using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Calcpad.web.Data;
using Calcpad.web.Data.Models;

namespace Calcpad.web.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subscription
        public async Task<IActionResult> Index()
        {
            var subscriptions = await _context.SubscriptionPlans.ToListAsync();
            return View(subscriptions);
        }

        // GET: Subscription/Order/5
        public async Task<IActionResult> Order(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscriptionPlan = await _context.SubscriptionPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subscriptionPlan == null)
            {
                return NotFound();
            }
            
            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.PlanId == id);

            return View(order);
        }

        // POST: Subscription/Order/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(int id, [Bind("Id,UserId,PlanId,CreatedOn,ActivatedOn,ExpiresOn")] Order order)
        {
            if (id != order.PlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionPlanExists(order.PlanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(order);
        }

        private bool SubscriptionPlanExists(int id)
        {
            return _context.SubscriptionPlans.Any(e => e.Id == id);
        }
    }
}