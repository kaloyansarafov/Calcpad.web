using Calcpad.web.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calcpad.web.Data.Services
{
    /// <summary>
    /// Interface for the Worksheet Service.
    /// </summary>
    public interface IWorksheetService
    {
        /// <summary>
        /// Adds a new worksheet.
        /// </summary>
        /// <param name="worksheet">The worksheet to add.</param>
        /// <returns>The added worksheet.</returns>
        public Task<Worksheet> AddAsync(Worksheet worksheet);

        /// <summary>
        /// Gets a worksheet by its ID.
        /// </summary>
        /// <param name="id">The ID of the worksheet.</param>
        /// <returns>The worksheet with the given ID.</returns>
        public Task<Worksheet> GetByIdAsync(int id);

        /// <summary>
        /// Gets the source code of a worksheet by its ID.
        /// </summary>
        /// <param name="id">The ID of the worksheet.</param>
        /// <returns>The source code of the worksheet with the given ID.</returns>
        public Task<string> GetSourceCodeAsync(int id);

        /// <summary>
        /// Updates an existing worksheet.
        /// </summary>
        /// <param name="worksheet">The updated worksheet.</param>
        /// <returns>The updated worksheet.</returns>
        public Task<Worksheet> UpdateAsync(Worksheet worksheet);

        /// <summary>
        /// Deletes a worksheet by its ID.
        /// </summary>
        /// <param name="id">The ID of the worksheet to delete.</param>
        /// <returns>The deleted worksheet.</returns>
        public Task<Worksheet> DeleteAsync(int id);

        /// <summary>
        /// Searches for worksheets that match the given search term.
        /// </summary>
        /// <param name="seachTerm">The search term to match worksheets against.</param>
        /// <returns>A list of worksheets that match the search term.</returns>
        public Task<IEnumerable<Worksheet>> SearchAsync(string seachTerm);
    }
}