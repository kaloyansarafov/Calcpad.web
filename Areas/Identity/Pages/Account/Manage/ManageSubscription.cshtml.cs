using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Calcpad.web.Data.Services;
using Calcpad.web.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Calcpad.web.Areas.Identity.Pages.Account.Manage
{
    public class ManageSubscriptionModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderService _orderService;

        public ManageSubscriptionModel(UserManager<ApplicationUser> userManager, IOrderService orderService)
        {
            _userManager = userManager;
            _orderService = orderService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public Order LatestOrder { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            LatestOrder = await _orderService.GetLastByUserIdAsync(user.Id);

            if (LatestOrder == null)
            {
                StatusMessage = "You have no orders yet.";
            }

            return Page();
        }
        
        public async Task<IActionResult> OnPostCancelAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            LatestOrder = await _orderService.GetLastByUserIdAsync(user.Id);

            if (LatestOrder == null)
            {
                StatusMessage = "You have no orders yet.";
                return Page();
            }

            LatestOrder.Invoice.IsCanceled = true;
            await _orderService.UpdateAsync(LatestOrder);

            StatusMessage = "Your subscription has been cancelled.";

            return RedirectToPage();
        }
    }
}