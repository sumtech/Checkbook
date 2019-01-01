// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Controllers
{
    using System.Collections.Generic;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for managing categories.
    /// </summary>
    [Produces("application/json")]
    public class CategoriesController : ControllerBase
    {
        /// <summary>
        /// The repository for managing categories.
        /// </summary>
        private readonly ICategoriesRepository categoriesRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="categoriesRepository">The repository for managing categories.</param>
        public CategoriesController(ICategoriesRepository categoriesRepository)
        {
            this.categoriesRepository = categoriesRepository;
        }

        /// <summary>
        /// Gets the list of all categories for the user.
        /// </summary>
        /// <returns>The list of categories.</returns>
        [HttpGet("api/categories")]
        [ProducesResponseType(typeof(List<Category>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult Get()
        {
            long userId = 1;

            IEnumerable<Category> categories;
            try
            {
                categories = this.categoriesRepository.GetAll(userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the categories.");
            }

            if (categories == null)
            {
                return this.Ok(new List<Category>());
            }

            return this.Ok(categories);
        }

        /// <summary>
        /// Gets a specified category.
        /// </summary>
        /// <param name="categoryId">The unique ID for the category.</param>
        /// <returns>The category.</returns>
        [HttpGet("api/categories/{categoryId:long}")]
        [ProducesResponseType(typeof(List<Category>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Get(long categoryId)
        {
            long userId = 1;

            Category category;
            try
            {
                category = this.categoriesRepository.Get(categoryId, userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the category.");
            }

            if (category == null)
            {
                return this.NotFound();
            }

            return this.Ok(category);
        }

        /// <summary>
        /// Adds a category.
        /// </summary>
        /// <param name="category">The category information.</param>
        /// <returns>The saved category.</returns>
        [HttpPost("api/categories")]
        [ProducesResponseType(typeof(List<Category>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Post([FromBody] Category category)
        {
            long userId = 1;
            if (category == null)
            {
                return this.BadRequest("A category must be passed in for it to be saved.");
            }

            category.UserId = userId;

            Category savedCategory;
            try
            {
                savedCategory = this.categoriesRepository.Add(category, userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error saving the category.");
            }

            return this.Ok(savedCategory);
        }

        /// <summary>
        /// Updates a category.
        /// </summary>
        /// <param name="categoryId">The unique ID for the category.</param>
        /// <param name="category">The category information.</param>
        /// <returns>The updated category.</returns>
        [HttpPut("api/categories/{categoryId:long}")]
        [ProducesResponseType(typeof(List<Category>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Put(long categoryId, [FromBody] Category category)
        {
            long userId = 1;

            if (category == null)
            {
                return this.BadRequest("A category must be passed in for it to be saved.");
            }

            if (category.Id != categoryId)
            {
                return this.BadRequest("The category ID values did not match.");
            }

            category.UserId = userId;

            Category savedCategory;
            try
            {
                savedCategory = this.categoriesRepository.Save(category, userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error saving the category.");
            }

            return this.Ok(savedCategory);
        }
    }
}
