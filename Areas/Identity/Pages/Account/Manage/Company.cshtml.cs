using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Calcpad.web.Data.Models;
using Calcpad.web.Data.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Calcpad.web.Areas.Identity.Pages.Account.Manage;

public class Company : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICompanyService _companyService;

    public Company(UserManager<ApplicationUser> userManager, ICompanyService companyService)
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
        var user = await _userManager.GetUserAsync(User);
        var company = await _companyService.GetByIdAsync(user.CompanyId);
        if (company == null)
        {
            return NotFound($"Unable to load company with ID '{user.CompanyId}'.");
        }
        
        Input = new InputModel
        {
            Name = company.Name,
            TaxRegistrationNumber = company.TaxRegistrationNumber,
            VATRegistered = company.VATRegistered,
            VATNumber = company.VATNumber,
            AccountablePerson = company.AccountablePerson,
            City = company.City,
            ZipCode = company.ZipCode,
            Address = company.Address,
            Country = Bia.Countries.Iso3166.Countries.GetCountryByAlpha2(company.Country).ActiveDirectoryName
        };
        
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
        var company = user.Company;
        if (company == null)
        {
            return NotFound($"Unable to load company with ID '{user.CompanyId}'.");
        }
        
        company.Name = Input.Name;
        company.TaxRegistrationNumber = Input.TaxRegistrationNumber;
        company.VATRegistered = Input.VATRegistered;
        company.VATNumber = Input.VATNumber;
        company.AccountablePerson = Input.AccountablePerson;
        company.City = Input.City;
        company.ZipCode = Input.ZipCode;
        company.Address = Input.Address;
        company.Country = Input.Country;
        
        await _companyService.UpdateAsync(company);
        
        StatusMessage = "Your company details have been updated";
        return RedirectToPage();
    }
}