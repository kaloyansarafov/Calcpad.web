using System.Threading.Tasks;
using Calcpad.web.Data.Models;

namespace Calcpad.web.Data.Services;

public interface IInvoiceService
{
    public Task<Invoice> GetByIdAsync(int id);
    public Task<Invoice> AddAsync(Invoice invoice);
    public Task<Invoice> UpdateAsync(Invoice invoice);
    public Task<Invoice> DeleteAsync(int id);
}