using Calcpad.web.Data.Models;
using System.Threading.Tasks;

namespace Calcpad.web.Data.Services
{
    /// <summary>
    /// Interface for the Company Service.
    /// </summary>
    public interface ICompanyService
    {
        /// <summary>
        /// Adds a new company.
        /// </summary>
        /// <param name="company">The company to add.</param>
        /// <returns>The added company.</returns>
        public Task<Company> AddAsync(Company company);

        /// <summary>
        /// Gets a company by its ID.
        /// </summary>
        /// <param name="id">The ID of the company.</param>
        /// <returns>The company with the given ID.</returns>
        public Task<Company> GetByIdAsync(int? id);

        /// <summary>
        /// Updates an existing company.
        /// </summary>
        /// <param name="company">The updated company.</param>
        /// <returns>The updated company.</returns>
        public Task<Company> UpdateAsync(Company company);

        /// <summary>
        /// Deletes a company by its ID.
        /// </summary>
        /// <param name="id">The ID of the company to delete.</param>
        /// <returns>The deleted company.</returns>
        public Task<Company> DeleteAsync(int id);
    }
}