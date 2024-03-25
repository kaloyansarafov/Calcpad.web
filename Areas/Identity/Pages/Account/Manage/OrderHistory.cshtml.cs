using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Calcpad.web.Data.Models;
using Calcpad.web.Data.Services;
using Microsoft.AspNetCore.Identity;

namespace Calcpad.web.Areas.Identity.Pages.Account.Manage;

public class OrderHistory : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOrderService _orderService;
    
    //ctor
    public OrderHistory(UserManager<ApplicationUser> userManager, IOrderService orderService)
    {
        _userManager = userManager;
        _orderService = orderService;
    }
    
    [TempData]
    public string StatusMessage { get; set; }
    
    public List<Order> Orders { get; set; }
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        Orders = await _orderService.GetByUserIdAsync(user.Id);
        Orders = Orders.OrderByDescending(o => o.CreatedOn).ToList();
        return Page();
    }
}