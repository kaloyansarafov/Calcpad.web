using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calcpad.web.Data;
using Calcpad.web.Data.Models;
using Calcpad.web.Data.Services;
using Calcpad.web.Global;
using Microsoft.EntityFrameworkCore;

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
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(24));
        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        Console.WriteLine($"SubscriptionExpirationService is working. Time: {DateTime.UtcNow}");
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        HandleStartingSubscriptions(context, userManager, orderService);
        HandleExpiredSubscriptions(context, userManager, orderService);
    }
    
    private void HandleStartingSubscriptions(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOrderService orderService)
    {
        var startingOrders = context.Orders
            .Where(o => o.ActivatedOn.Date == DateTime.UtcNow.Date && o.IsActive == false).Include(order => order.User)
            .ToList();

        foreach (var order in startingOrders)
        {
            userManager.AddToRoleAsync(order.User, Constants.RoleNames.Subscriber).Wait();
            order.IsActive = true;
            orderService.UpdateAsync(order).Wait();
        }
    }

    private void HandleExpiredSubscriptions(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IOrderService orderService)
    {
        var expiredOrders = context.Orders
            .Where(o => o.ExpiresOn.Date == DateTime.UtcNow.Date.AddDays(-1) && o.IsActive == true).Include(order => order.User)
            .ToList();

        foreach (var order in expiredOrders)
        {
            userManager.RemoveFromRoleAsync(order.User, Constants.RoleNames.Subscriber).Wait();
            order.IsActive = false;
            orderService.UpdateAsync(order).Wait();
        }
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