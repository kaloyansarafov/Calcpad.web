﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Calcpad.web.Data;
using Calcpad.web.Data.Models;
using Calcpad.web.Data.Services;
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
        public async Task<IActionResult> Order(int id, [Bind("ActivatedOn")] Order order)
        {
            order.PlanId = id;
            order.Plan = await _context.SubscriptionPlans.FirstOrDefaultAsync(m => m.Id == id);
            order.User = await _userManager.GetUserAsync(User);
            order.CreatedOn = DateTime.Now;
            order.ExpiresOn = order.ActivatedOn.AddMonths(1);

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

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.AddAsync(order);

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