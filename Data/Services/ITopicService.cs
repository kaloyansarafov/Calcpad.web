using Calcpad.web.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calcpad.web.Data.Services
{
    /// <summary>
    /// Interface for the Topic Service.
    /// </summary>
    public interface ITopicService
    {
        /// <summary>
        /// Adds a new topic.
        /// </summary>
        /// <param name="topic">The topic to add.</param>
        /// <returns>The added topic.</returns>
        public Task<Topic> AddAsync(Topic topic);

        /// <summary>
        /// Gets a topic by its ID.
        /// </summary>
        /// <param name="id">The ID of the topic.</param>
        /// <returns>The topic with the given ID.</returns>
        public Task<Topic> GetByIdAsync(int id);

        /// <summary>
        /// Gets all topics.
        /// </summary>
        /// <returns>A list of all topics.</returns>
        public Task<List<Topic>> GetAllAsync();

        /// <summary>
        /// Renames an existing topic.
        /// </summary>
        /// <param name="id">The ID of the topic to rename.</param>
        /// <param name="name">The new name of the topic.</param>
        /// <returns>The renamed topic.</returns>
        public Task<Topic> RenameAsync(int id, string name);

        /// <summary>
        /// Deletes a topic by its ID.
        /// </summary>
        /// <param name="id">The ID of the topic to delete.</param>
        /// <returns>The deleted topic.</returns>
        public Task<Topic> DeleteAsync(int id);
    }
}