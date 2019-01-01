// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a budget to which finances are being allocated for a future
    /// expense and includes summary information for the budget.
    /// </summary>
    public class BudgetSummary : Budget
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BudgetSummary"/> class.
        /// </summary>
        /// <param name="budget">The base budget information.</param>
        public BudgetSummary(Budget budget)
        {
            this.Id = budget.Id;
            this.Name = budget.Name;
            this.CategoryId = budget.CategoryId;
            this.Category = budget.Category;
            this.UserId = budget.UserId;
            this.User = budget.User;

            // TODO: We need to be calculating the balance in the database,
            // not here. I am using this code for now since SQLite does not
            // support stored procedures and I have not started using an actual
            // SQL Server database yet. When I do, I should refactor this code
            // and create a new reporting service class for handling report
            // data retrieval.
            if (budget.TransactionItems != null)
            {
                this.Balance = budget.TransactionItems
                    .Sum(ti => ti.Amount);
            }

            budget.Category.Budgets = new List<Budget>();
        }

        /// <summary>
        /// Gets or sets the current balance value stored in the budget.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
        public decimal Balance { get; set; }
    }
}
