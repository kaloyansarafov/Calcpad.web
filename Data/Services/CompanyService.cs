using System.Threading.Tasks;
using Calcpad.web.Data.Models;

namespace Calcpad.web.Data.Services;

public class CompanyService : ICompanyService
{
    private readonly ApplicationDbContext _context;
    
    public CompanyService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Company> AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
        return company;
    }
    
    public async Task<Company> GetByIdAsync(int? id)
    {
        return await _context.Companies.FindAsync(id);
    }
    
    public async Task<Company> UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
        return company;
    }
    
    public async Task<Company> DeleteAsync(int id)
    {
        Company company = await GetByIdAsync(id);
        if (company != null)
        {
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
        }
        return company;
    }
}