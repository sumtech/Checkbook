// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Tests.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Query;

    /// <summary>
    /// Service for getting data collections from a context for testing.
    /// </summary>
    public class ContextDataService
    {
        /// <summary>
        /// Gets the set of account information from the context with child objects.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>The list of accounts.</returns>
        public static List<Account> GetAccounts(CheckbookContext context)
        {
            return GetAccountsSet(context).ToList();
        }

        /// <summary>
        /// Gets the database set for accounts with the appropriate includes.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>The accounts database set.</returns>
        public static IQueryable<Account> GetAccountsSet(CheckbookContext context)
        {
            return context.Accounts;
        }

        /// <summary>
        /// Gets the set of budget information from the context with child objects.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>The list of budgets.</returns>
        public static List<Budget> GetBudgets(CheckbookContext context)
        {
            return GetBudgetsSet(context).ToList();
        }

        /// <summary>
        /// Gets the database set for budgets with the appropriate includes.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>The budgets database set.</returns>
        public static IQueryable<Budget> GetBudgetsSet(CheckbookContext context)
        {
            return context.Budgets
                .Include(b => b.Category);
        }

        /// <summary>
        /// Gets the set of category information from the context with child objects.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>The list of categories.</returns>
        public static List<Category> GetCategories(CheckbookContext context)
        {
            return GetCategoriesSet(context).ToList();
        }

        /// <summary>
        /// Gets the database set for categories with the appropriate includes.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>The categories database set.</returns>
        public static IQueryable<Category> GetCategoriesSet(CheckbookContext context)
        {
            return context.Categories;
        }

        /// <summary>
        /// Gets the set of transaction information from the context with child objects.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>The list of transactions.</returns>
        public static List<Transaction> GetTransactions(CheckbookContext context)
        {
            return GetTransactionsSet(context).ToList();
        }

        /// <summary>
        /// Gets the database set for transactions with the appropriate includes.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <returns>The transactions database set.</returns>
        public static IQueryable<Transaction> GetTransactionsSet(CheckbookContext context)
        {
            return context.Transactions
                .Include(t => t.FromAccount)
                .Include(t => t.ToAccount)
                .Include(t => t.Items)
                    .ThenInclude(i => i.Budget)
                        .ThenInclude(s => s.Category);
        }
    }
}
