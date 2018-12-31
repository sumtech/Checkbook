// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Checkbook.Api.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    /// <summary>
    /// A repository for managing categories.
    /// </summary>
    public class CategoriesRepository : ICategoriesRepository
    {
        /// <summary>
        /// The context for communicating with the checkbook database.
        /// </summary>
        private readonly CheckbookContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesRepository"/> class.
        /// </summary>
        /// <param name="context">The context for communicating with the checkbook database.</param>
        public CategoriesRepository(CheckbookContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the list of categories.
        /// </summary>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>A list of categories.</returns>
        public IEnumerable<Category> GetAll(long userId)
        {
            return this.context.Categories
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.Name)
                .AsEnumerable();
        }

        /// <summary>
        /// Gets a specified category record.
        /// </summary>
        /// <param name="categoryId">The unique identifier for the category.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The category.</returns>
        public Category Get(long categoryId, long userId)
        {
            if (userId == 0)
            {
                throw new ArgumentException("", "userId");
            }

            Category category = this.context.Categories
                .Where(a => a.UserId == userId)
                .SingleOrDefault(a => a.Id == categoryId);

            if (category == null)
            {
                throw new NotFoundException("The category was not found.");
            }

            return category;
        }

        /// <summary>
        /// Add a new category to the data store.
        /// </summary>
        /// <param name="category">The new category to add.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved category with the updated identifier.</returns>
        public Category Add(Category category, long userId)
        {
            // Verify we do not have an ID set, which would indicate the Save method should have been used.
            if (category.Id != 0)
            {
                throw new ArgumentException("A new category without a specified ID should have been used. To update a category, use the Save method.", "category.Id");
            }

            // Verify we have the correct user information.
            if (userId == 0)
            {
                throw new ArgumentException("A user ID is expected to be passed in.", "userId");
            }

            if (category.UserId == 0)
            {
                throw new ArgumentException("A category is expected to have a user ID.", "category.UserId");
            }

            if (category.UserId != userId)
            {
                throw new ArgumentException("A user ID is expected to match the passed in user ID for a category.", "category.UserId");
            }

            // Save the new category.
            EntityEntry<Category> savedCategory = this.context.Categories.Add(category);
            this.context.SaveChanges();
            return savedCategory.Entity;
        }

        /// <summary>
        /// Saves updates to a category.
        /// </summary>
        /// <param name="category">The category to be saved.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved category information.</returns>
        public Category Save(Category category, long userId)
        {
            // Verify we do have an ID set.
            if (category.Id == 0)
            {
                throw new ArgumentException("A category with a specified ID should have been used. To add a category, use the Add method.", "category.Id");
            }

            // Verify we have the correct user information.
            if (userId == 0)
            {
                throw new ArgumentException("A user ID is expected to be passed in.", "userId");
            }

            if (category.UserId == 0)
            {
                throw new ArgumentException("A category is expected to have a user ID.", "category.UserId");
            }

            if (category.UserId != userId)
            {
                throw new ArgumentException("A user ID is expected to match the passed in user ID for a category.", "category.UserId");
            }

            // Save the new category.
            this.context.Entry(category).State = EntityState.Modified;
            this.context.SaveChanges();

            return category;
        }
    }
}
