// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System.Collections.Generic;
    using Checkbook.Api.Models;

    /// <summary>
    /// A repository for managing categories.
    /// </summary>
    public interface ICategoriesRepository
    {
        /// <summary>
        /// Gets the list of categories.
        /// </summary>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>A list of categories.</returns>
        IEnumerable<Category> GetAll(long userId);

        /// <summary>
        /// Gets a specified category record.
        /// </summary>
        /// <param name="categoryId">The unique identifier for the category.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The category.</returns>
        Category Get(long categoryId, long userId);

        /// <summary>
        /// Add a new category to the data store.
        /// </summary>
        /// <param name="category">The new category to add.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved category with the updated identifier.</returns>
        Category Add(Category category, long userId);

        /// <summary>
        /// Saves updates to a category.
        /// </summary>
        /// <param name="category">The category to be saved.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved category information.</returns>
        Category Save(Category category, long userId);
    }
}
