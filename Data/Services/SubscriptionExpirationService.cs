using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calcpad.web.Data.Models;
using Calcpad.web.Global;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Calcpad.web.Data.Services;

/// <summary>
/// Service for handling subscription expiration.
/// </summary>
public class SubscriptionExpirationService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;

    public SubscriptionExpirationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(5), TimeSpan.FromHours(24));
        return Task.CompletedTask;
    }

    private async void DoWork(object state)
    {
        Console.WriteLine($"SubscriptionExpirationService is working. Time: {DateTime.UtcNow}");
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        await HandleStartingSubscriptions(context, userManager, orderService);
        await HandleExpiredSubscriptions(context, userManager, orderService);
    }
    
    /// <summary>
    /// Service for handling subscription expiration.
    /// </summary>
    private async Task HandleStartingSubscriptions(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOrderService orderService)
    {
        var startingOrders = await orderService.GetOrdersToStartAsync();

        foreach (var order in startingOrders)
        {
            await userManager.AddToRoleAsync(order.User, Constants.RoleNames.Subscriber);
            order.IsActive = true;
            await orderService.UpdateAsync(order);
        }
    }

    /// <summary>
    /// Handles expired subscriptions.
    /// </summary>
    private async Task HandleExpiredSubscriptions(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOrderService orderService)
    {
        var expiredOrders = await orderService.GetOrdersToExpireAsync();

        foreach (var order in expiredOrders)
        {
            await userManager.RemoveFromRoleAsync(order.User, Constants.RoleNames.Subscriber);
            order.IsActive = false;
            await orderService.UpdateAsync(order);

            if (InvoiceStillActive(order))
            {
                //renew subscription with a new order and invoice
                var newOrder = new Order
                {
                    PlanId = order.PlanId,
                    User = order.User,
                    CreatedOn = DateTime.Now,
                    ActivatedOn = DateTime.Today,
                    ExpiresOn = DateTime.Today.AddMonths(1),
                    IsActive = true
                };
                
                var invoice = new Invoice
                {
                    InvoiceNumber = (await context.Invoices.OrderByDescending(i => i.InvoiceNumber).FirstOrDefaultAsync())?.InvoiceNumber + 1 ?? 1,
                    NetAmmount = order.Plan.Price,
                    TaxAmmount = order.Plan.Price * 0.21m,
                    CreatedDate = DateTime.Now,
                    PaymentDate = newOrder.ActivatedOn,
                    IsCanceled = false
                };
                
                newOrder.Invoice = invoice;
                
                await orderService.AddAsync(newOrder);
                
                await userManager.AddToRoleAsync(order.User, Constants.RoleNames.Subscriber);
            }
        }
    }
    
    private bool InvoiceStillActive(Order order)
    {
        if (order.Invoice.IsCanceled)
            return false;

        return true;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}