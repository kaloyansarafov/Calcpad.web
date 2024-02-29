using AutoMapper;
using Calcpad.web.Areas.Admin.Models;
using Calcpad.web.Data.Models;
using Calcpad.web.Data.Services;
using Calcpad.web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Calcpad.web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Global.Constants.RoleNames.Administrator)]
    public class SubscriptionController : Controller
    {
        private readonly ISubscriptionPlanService _subscriptionPlanService;
        private readonly IMapper _mapper;

        public SubscriptionController(ISubscriptionPlanService subscriptionPlanService, IMapper mapper)
        {
            _subscriptionPlanService = subscriptionPlanService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            SubscriptionPlanViewModel model = new();
            return View(nameof(Update), model);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (!id.HasValue)
                return BadRequest();

            SubscriptionPlan subscriptionPlan = await _subscriptionPlanService.GetByIdAsync(id.Value);
            if (subscriptionPlan == null)
                return NotFound();

            SubscriptionPlanViewModel model = _mapper.Map<SubscriptionPlanViewModel>(subscriptionPlan);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            SubscriptionPlan subscriptionPlan = await _subscriptionPlanService.GetByIdAsync(id.Value);
            if (subscriptionPlan == null)
                return NotFound();

            SubscriptionPlanViewModel model = _mapper.Map<SubscriptionPlanViewModel>(subscriptionPlan);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SubscriptionPlanViewModel model)
        {
            model.Id = 0;

            if (!ModelState.IsValid)
                return View(nameof(Update), model);

            SubscriptionPlan subscriptionPlan = _mapper.Map<SubscriptionPlan>(model);
            await _subscriptionPlanService.AddAsync(subscriptionPlan);
            return RedirectToAction(string.Empty, "help", new { Area = "", Controller = "Index", subscriptionPlan.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Update(SubscriptionPlanViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            SubscriptionPlan subscriptionPlan = _mapper.Map<SubscriptionPlan>(model);
            await _subscriptionPlanService.UpdateAsync(subscriptionPlan);
            return RedirectToAction(string.Empty, "help", new { Area = "", Controller = "Index", subscriptionPlan.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(SubscriptionPlanViewModel model)
        {
            await _subscriptionPlanService.DeleteAsync(model.Id);
            return RedirectToAction(string.Empty, "help", new { Area = "", Controller = "Index"});
        }
    }
}