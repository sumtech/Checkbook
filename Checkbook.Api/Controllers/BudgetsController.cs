﻿// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Controllers
{
    using System.Collections.Generic;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// The API controller for managing budgets.
    /// </summary>
    [Produces("application/json")]
    public class BudgetsController : ControllerBase
    {
        /// <summary>
        /// The repository for managing budgets.
        /// </summary>
        private readonly IBudgetsRepository budgetsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetsController"/> class.
        /// </summary>
        /// <param name="budgetsRepository">The repository for managing budgets.</param>
        public BudgetsController(IBudgetsRepository budgetsRepository)
        {
            this.budgetsRepository = budgetsRepository;
        }

        /// <summary>
        /// Gets the list of all bank budgets or the user.
        /// </summary>
        /// <returns>The list of budgets.</returns>
        [HttpGet("api/budgets")]
        [ProducesResponseType(typeof(List<Budget>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        public IActionResult GetAll()
        {
            long userId = 1;

            IEnumerable<Budget> budgets;
            try
            {
                budgets = this.budgetsRepository.GetAll(userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the budgets.");
            }

            return this.Ok(budgets);
        }

        /// <summary>
        /// Gets a specified budget.
        /// </summary>
        /// <param name="budgetId">The unique ID for the budget.</param>
        /// <returns>The budget.</returns>
        [HttpGet("api/budgets/{id:long}")]
        [ProducesResponseType(typeof(List<Budget>), 200)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(404)]
        public IActionResult Get(long budgetId)
        {
            long userId = 1;

            Budget budget;
            try
            {
                budget = this.budgetsRepository.Get(budgetId, userId);
            }
            catch
            {
                return this.StatusCode(500, "There was an error getting the budget.");
            }

            if (budget == null)
            {
                return this.NotFound();
            }

            return this.Ok(budget);
        }
    }
}