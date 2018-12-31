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
    /// A repository for managing budgets.
    /// </summary>
    public class BudgetsRepository : IBudgetsRepository
    {
        /// <summary>
        /// The context for communicating with the checkbook database.
        /// </summary>
        private readonly CheckbookContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetsRepository"/> class.
        /// </summary>
        /// <param name="context">The context for communicating with the checkbook database.</param>
        public BudgetsRepository(CheckbookContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets the list of budgets.
        /// </summary>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>A list of budgets.</returns>
        public IEnumerable<Budget> GetAll(long userId)
        {
            return this.context.Budgets
                .Include(b => b.Category)
                .Where(b => b.UserId == userId)
                .OrderBy(b => b.Category.Name)
                    .ThenBy(b => b.Name)
                .AsEnumerable();
        }

        /// <summary>
        /// Gets a specified budget record.
        /// </summary>
        /// <param name="budgetId">The unique identifier for the budget.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The budget.</returns>
        public Budget Get(long budgetId, long userId)
        {
            if (userId == 0)
            {
                throw new ArgumentException("", "userId");
            }

            Budget budget = this.context.Budgets
                .Where(a => a.UserId == userId)
                .SingleOrDefault(a => a.Id == budgetId);

            if (budget == null)
            {
                throw new NotFoundException("The budget was not found.");
            }

            return budget;
        }

        /// <summary>
        /// Add a new budget to the data store.
        /// </summary>
        /// <param name="budget">The new budget to add.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved budget with the updated identifier.</returns>
        public Budget Add(Budget budget, long userId)
        {
            // Verify we do not have an ID set, which would indicate the Save method should have been used.
            if (budget.Id != 0)
            {
                throw new ArgumentException("A new budget without a specified ID should have been used. To update a budget, use the Save method.", "budget.Id");
            }

            // Verify we have the correct user information.
            if (userId == 0)
            {
                throw new ArgumentException("A user ID is expected to be passed in.", "userId");
            }

            if (budget.UserId == 0)
            {
                throw new ArgumentException("A budget is expected to have a user ID.", "budget.UserId");
            }

            if (budget.UserId != userId)
            {
                throw new ArgumentException("A user ID is expected to match the passed in user ID for a budget.", "budget.UserId");
            }

            // Save the new budget.
            EntityEntry<Budget> savedBudget = this.context.Budgets.Add(budget);
            this.context.SaveChanges();
            return savedBudget.Entity;
        }

        /// <summary>
        /// Saves updates to a budget.
        /// </summary>
        /// <param name="budget">The budget to be saved.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved budget information.</returns>
        public Budget Save(Budget budget, long userId)
        {
            // Verify we do have an ID set.
            if (budget.Id == 0)
            {
                throw new ArgumentException("A budget with a specified ID should have been used. To add a budget, use the Add method.", "budget.Id");
            }

            // Verify we have the correct user information.
            if (userId == 0)
            {
                throw new ArgumentException("A user ID is expected to be passed in.", "userId");
            }

            if (budget.UserId == 0)
            {
                throw new ArgumentException("A budget is expected to have a user ID.", "budget.UserId");
            }

            if (budget.UserId != userId)
            {
                throw new ArgumentException("A user ID is expected to match the passed in user ID for a budget.", "budget.UserId");
            }

            // Save the new budget.
            this.context.Entry(budget).State = EntityState.Modified;
            this.context.SaveChanges();

            return budget;
        }
    }
}
