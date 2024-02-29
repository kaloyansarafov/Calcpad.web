using Microsoft.AspNetCore.Mvc;

namespace Calcpad.web.Areas.Admin.Controllers;

public class SubscriptionController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}