using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Calcpad.web.Data;
using Calcpad.web.Data.Models;
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

        var expiredOrders = context.Orders
            .Where(o => o.ExpiresOn.Date == DateTime.UtcNow.Date.AddDays(-1)).Include(order => order.User)
            .ToList();

        foreach (var order in expiredOrders)
        {
            userManager.RemoveFromRoleAsync(order.User, Constants.RoleNames.Subscriber).Wait();
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