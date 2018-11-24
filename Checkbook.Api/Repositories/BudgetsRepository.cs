// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
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
        /// Gets the list of bank budgets.
        /// </summary>
        /// <param name="userId">The unique identifier cfor the current user.</param>
        /// <returns>A list of bank budgets.</returns>
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
        /// Add a new budget to the data store.
        /// </summary>
        /// <param name="budget">The new budget to add.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved budget with the updated identifier.</returns>
        public Budget Add(Budget budget, long userId)
        {
            EntityEntry<Budget> savedBudget = this.context.Budgets.Add(budget);
            this.context.SaveChanges();
            return savedBudget.Entity;
        }

        /// <summary>
        /// Gets a specified budget record.
        /// </summary>
        /// <param name="budgetId">The unique identifier for the budget.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The budget.</returns>
        public Budget Get(long budgetId, long userId)
        {
            return this.context.Budgets
                .Where(a => a.UserId == userId)
                .Single(a => a.Id == budgetId);
        }
    }
}
