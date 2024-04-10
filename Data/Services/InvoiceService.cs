using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Calcpad.web.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Calcpad.web.Data.Services;

public class InvoiceService : IInvoiceService
{
    private readonly ApplicationDbContext _context;

    public InvoiceService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Invoice> AddAsync(Invoice invoice)
    {
        await _context.Invoices.AddAsync(invoice);
        await _context.SaveChangesAsync();
        return invoice;
    }

    public async Task<Invoice> DeleteAsync(int id)
    {
        Invoice invoice = await _context.Invoices.FindAsync(id);
        if (invoice != null)
        {
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }
        return invoice;
    }

    public async Task<Invoice> GetByIdAsync(int id)
    {
        if (id == 0)
            return await _context.Invoices.FirstOrDefaultAsync();

        return await _context.Invoices
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Invoice> UpdateAsync(Invoice invoice)
    {
        EntityEntry invoiceEntry = _context.Invoices.Attach(invoice);
        invoiceEntry.State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return invoice;
    }
    
    public async Task<IEnumerable<Invoice>> GetAllAsync()
    {
        return await _context.Invoices.ToListAsync();
    }
}