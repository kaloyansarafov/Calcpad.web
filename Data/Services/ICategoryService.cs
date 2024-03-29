using Calcpad.web.Data.Models;
using System.Threading.Tasks;

namespace Calcpad.web.Data.Services
{
    /// <summary>
    /// Interface for the Category Service.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Adds a new category.
        /// </summary>
        /// <param name="category">The category to add.</param>
        /// <returns>The added category.</returns>
        public Task<Category> AddAsync(Category category);

        /// <summary>
        /// Gets a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>The category with the given ID.</returns>
        public Task<Category> GetByIdAsync(int id);

        /// <summary>
        /// Renames an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to rename.</param>
        /// <param name="name">The new name of the category.</param>
        /// <returns>The renamed category.</returns>
        public Task<Category> RenameAsync(int id, string name);

        /// <summary>
        /// Deletes a category by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>The deleted category.</returns>
        public Task<Category> DeleteAsync(int id);
    }
}