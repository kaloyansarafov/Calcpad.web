﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Calcpad.web.Data;
using Calcpad.web.Data.Models;
using Calcpad.web.Data.Services;
using Calcpad.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Calcpad.web.Global;

namespace Calcpad.web.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public SubscriptionController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOrderService orderService, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _orderService = orderService;
            _mapper = mapper;
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
            var user = await _userManager.GetUserAsync(User);
            
            // Check if the user is already a subscriber
            var activeOrder = await _context.Orders
                .Where(o => o.User.Id == user.Id && o.IsActive == true)
                .FirstOrDefaultAsync();
            if (activeOrder != null)
            {
                ModelState.AddModelError("ActivatedOn", "You already have an active subscription.");
                return View(order);
            }
            
            var plan = await _context.SubscriptionPlans.FirstOrDefaultAsync(m => m.Id == id);
            
            order.PlanId = id;
            order.Plan = plan;
            order.User = user;
            order.CreatedOn = DateTime.Now;
            order.ExpiresOn = order.ActivatedOn.AddDays(plan.Days);
            order.IsActive = false;

            var invoice = new Invoice
            {
                InvoiceNumber = (await _context.Invoices.OrderByDescending(i => i.InvoiceNumber).FirstOrDefaultAsync())?.InvoiceNumber + 1 ?? 1,
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

                    if (order.ActivatedOn.Date >= DateTime.Now.Date)
                    {
                        await _userManager.AddToRoleAsync(user, Constants.RoleNames.Subscriber);
                        
                        order.IsActive = true;
                        await _orderService.UpdateAsync(order);
                    }

                    return RedirectToAction("OrderSuccessful", order);
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
        public async Task<IActionResult> OrderSuccessful(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var order = await _orderService.GetByIdAsync(id);
            
            if (user != null && user.Id != order.User.Id)
            {
                return Unauthorized();
            }
            
            if (order == null)
            {
                return NotFound();
            }
            
            OrderViewModel model = _mapper.Map<OrderViewModel>(order);

            return View(model);
        }

        private bool SubscriptionPlanExists(int id)
        {
            return _context.SubscriptionPlans.Any(e => e.Id == id);
        }
    }
}