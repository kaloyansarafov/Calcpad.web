using System.Threading.Tasks;
using Calcpad.web.Data.Models;

namespace Calcpad.web.Data.Services
{
    /// <summary>
    /// Interface for the Invoice Service.
    /// </summary>
    public interface IInvoiceService
    {
        /// <summary>
        /// Gets an invoice by its ID.
        /// </summary>
        /// <param name="id">The ID of the invoice.</param>
        /// <returns>The invoice with the given ID.</returns>
        public Task<Invoice> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new invoice.
        /// </summary>
        /// <param name="invoice">The invoice to add.</param>
        /// <returns>The added invoice.</returns>
        public Task<Invoice> AddAsync(Invoice invoice);

        /// <summary>
        /// Updates an existing invoice.
        /// </summary>
        /// <param name="invoice">The updated invoice.</param>
        /// <returns>The updated invoice.</returns>
        public Task<Invoice> UpdateAsync(Invoice invoice);

        /// <summary>
        /// Deletes an invoice by its ID.
        /// </summary>
        /// <param name="id">The ID of the invoice to delete.</param>
        /// <returns>The deleted invoice.</returns>
        public Task<Invoice> DeleteAsync(int id);
    }
}