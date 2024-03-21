using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Calcpad.web.Data;
using Calcpad.web.Data.Models;
using Calcpad.web.Data.Services;
using Calcpad.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Calcpad.web.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;

        public SubscriptionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOrderService orderService)
        {
            _context = context;
            _userManager = userManager;
            _orderService = orderService;
        }

        // GET: Subscription
        public async Task<IActionResult> Index()
        {
            var subscriptions = await _context.SubscriptionPlans.ToListAsync();
            return View(subscriptions);
        }

        // GET: Subscription/Order/5
        [Authorize]
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
    
            var order = new Order
            {
                PlanId = id.Value,
                User = await _userManager.GetUserAsync(User)
            };

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Order(int id, [Bind("ActivatedOn")] Order order)
        {
            order.PlanId = id;
            order.Plan = await _context.SubscriptionPlans.FirstOrDefaultAsync(m => m.Id == id);
            order.User = await _userManager.GetUserAsync(User);
            order.CreatedOn = DateTime.Now;
            order.ExpiresOn = order.ActivatedOn.AddMonths(1);
            order.IsActive = false;

            var invoice = new Invoice
            {
                InvoiceNumber = new Random().Next(1000, 9999),
                NetAmmount = order.Plan.Price,
                TaxAmmount = order.Plan.Price * 0.21m,
                CreatedDate = DateTime.Now,
                PaymentDate = order.ActivatedOn,
                IsCanceled = false
            };

            order.Invoice = invoice;
            
            // Check if the user is already a subscriber
            var activeOrder = await _context.Orders
                .Where(o => o.User.Id == order.User.Id && o.IsActive == true)
                .FirstOrDefaultAsync();
            if (activeOrder != null)
            {
                ModelState.AddModelError("ActivatedOn", "You already have an active subscription.");
                return View(order);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.AddAsync(order);

                    if (order.ActivatedOn.Date >= DateTime.Now.Date)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        await _userManager.AddToRoleAsync(user, "Subscriber");
                        
                        order.IsActive = true;
                        await _orderService.UpdateAsync(order);
                    }

                    // Serialize the Order object to a string
                    var orderJson = JsonSerializer.Serialize(order);

                    // Store the serialized Order object in TempData
                    TempData["Order"] = orderJson;

                    // Redirect to the OrderSuccessful action
                    return RedirectToAction("OrderSuccessful");
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
        
        // GET: Subscription/OrderSuccessful/5
        [Authorize]
        public async Task<IActionResult> OrderSuccessful()
        {
            // Retrieve the serialized Order object from TempData
            var orderJson = TempData.Peek("Order") as string;

            if (orderJson == null)
            {
                return NotFound();
            }

            // Deserialize the Order object
            var order = JsonSerializer.Deserialize<OrderViewModel>(orderJson);

            return View(order);
        }

        private bool SubscriptionPlanExists(int id)
        {
            return _context.SubscriptionPlans.Any(e => e.Id == id);
        }
    }
}