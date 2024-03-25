using System.Collections.Generic;
using System.Threading.Tasks;
using Calcpad.web.Data.Models;
using Calcpad.web.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Calcpad.web.Areas.Identity.Pages.Account.Manage
{
    public class RegisterCompanyModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyService _companyService;

        public RegisterCompanyModel(UserManager<ApplicationUser> userManager, ICompanyService companyService)
        {
            _userManager = userManager;
            _companyService = companyService;
        }

        public class InputModel
        {
            public string Name { get; set; }
            public string TaxRegistrationNumber { get; set; }
            public bool VATRegistered { get; set; }
            public int VATNumber { get; set; }
            public string AccountablePerson { get; set; }
            public string City { get; set; }
            public string ZipCode { get; set; }
            public string Address { get; set; }
            public string Country { get; set; }
        }
        
        [BindProperty]
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        
        public List<string> Countries { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Countries = Bia.Countries.Iso3166.Countries.GetAllActiveDirectoryNames();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Save the company details to the database here
            //map the input model to the company model
            var company = new Calcpad.web.Data.Models.Company
            {
                Name = Input.Name,
                TaxRegistrationNumber = Input.TaxRegistrationNumber,
                VATRegistered = Input.VATRegistered,
                VATNumber = Input.VATNumber,
                AccountablePerson = Input.AccountablePerson,
                Country = Bia.Countries.Iso3166.Countries.GetCountryByActiveDirectoryName(Input.Country).Alpha2.ToString(),
                City = Input.City,
                ZipCode = Input.ZipCode,
                Address = Input.Address
            };
            
            await _companyService.AddAsync(company);
            
            user.CompanyId = company.Id;
            await _userManager.UpdateAsync(user);

            StatusMessage = "Your company has been registered";
            return RedirectToPage();
        }
    }
}