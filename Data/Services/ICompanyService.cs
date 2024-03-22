using Calcpad.web.Data.Models;
using System.Threading.Tasks;

namespace Calcpad.web.Data.Services;

    public interface ICompanyService
    {
        public Task<Company> AddAsync(Company company);
        public Task<Company> GetByIdAsync(int id);
        public Task<Company> UpdateAsync(Company company);
        public Task<Company> DeleteAsync(int id);
    }