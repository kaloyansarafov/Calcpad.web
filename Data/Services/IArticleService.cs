using Calcpad.web.Data.Models;
using System.Threading.Tasks;

namespace Calcpad.web.Data.Services
{
    /// <summary>
    /// Interface for the Article Service.
    /// </summary>
    public interface IArticleService
    {
        /// <summary>
        /// Adds a new article.
        /// </summary>
        /// <param name="article">The article to add.</param>
        /// <returns>The added article.</returns>
        public Task<Article> AddAsync(Article article);

        /// <summary>
        /// Gets an article by its ID.
        /// </summary>
        /// <param name="id">The ID of the article.</param>
        /// <returns>The article with the given ID.</returns>
        public Task<Article> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing article.
        /// </summary>
        /// <param name="article">The updated article.</param>
        /// <returns>The updated article.</returns>
        public Task<Article> UpdateAsync(Article article);

        /// <summary>
        /// Deletes an article by its ID.
        /// </summary>
        /// <param name="id">The ID of the article to delete.</param>
        /// <returns>The deleted article.</returns>
        public Task<Article> DeleteAsync(int id);
    }
}