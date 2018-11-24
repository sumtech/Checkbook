// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Repositories
{
    using System.Collections.Generic;
    using Checkbook.Api.Models;

    /// <summary>
    /// A repository for managing budgets.
    /// </summary>
    public interface IBudgetsRepository
    {
        /// <summary>
        /// Gets the list of bank budgets.
        /// </summary>
        /// <param name="userId">The unique identifier cfor the current user.</param>
        /// <returns>A list of bank budgets.</returns>
        IEnumerable<Budget> GetAll(long userId);

        /// <summary>
        /// Add a new budget to the data store.
        /// </summary>
        /// <param name="budget">The new budget to add.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The saved budget with the updated identifier.</returns>
        Budget Add(Budget budget, long userId);

        /// <summary>
        /// Gets a specified budget record.
        /// </summary>
        /// <param name="budgetId">The unique identifier for the budget.</param>
        /// <param name="userId">The unique identifier for the current user.</param>
        /// <returns>The budget.</returns>
        Budget Get(long budgetId, long userId);
    }
}
