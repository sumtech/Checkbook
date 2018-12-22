// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Tests.Helpers
{
    using System;
    using System.Collections.Generic;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;

    /// <summary>
    /// Service for seeding database data while testing.
    /// </summary>
    public class DatabaseSeed
    {
        /// <summary>
        /// Adds entities to the context for testing.
        /// </summary>
        /// <param name="context">The database context.</param>
        public static void AddEntities(CheckbookContext context)
        {
            context.Users.Add(new User
            {
                Id = 1,
                FirstName = "First",
                LastName = "User",
                Username = "FirstUser",
            });
            context.Users.Add(new User
            {
                Id = 2,
                FirstName = "Second",
                LastName = "User",
                Username = "SecondUser",
            });

            context.Categories.Add(new Category
            {
                Id = 1,
                Name = "First Category for the first user",
                UserId = 1,
            });
            context.Categories.Add(new Category
            {
                Id = 2,
                Name = "Second Category for the first user",
                UserId = 1,
            });
            context.Categories.Add(new Category
            {
                Id = 3,
                Name = "First Category for the second user",
                UserId = 2,
            });
            context.Categories.Add(new Category
            {
                Id = 4,
                Name = "Second Category for the second user",
                UserId = 2,
            });

            context.Budgets.Add(new Budget
            {
                Id = 1,
                CategoryId = 1,
                Name = "First Budget for the first user",
                UserId = 1,
            });
            context.Budgets.Add(new Budget
            {
                Id = 2,
                CategoryId = 2,
                Name = "Second Budget for the first user",
                UserId = 1,
            });
            context.Budgets.Add(new Budget
            {
                Id = 3,
                CategoryId = 2,
                Name = "Third Budget for the first user",
                UserId = 1,
            });
            context.Budgets.Add(new Budget
            {
                Id = 4,
                CategoryId = 2,
                Name = "First Budget for the second user",
                UserId = 2,
            });
            context.Budgets.Add(new Budget
            {
                Id = 5,
                CategoryId = 2,
                Name = "Second Budget for the second user",
                UserId = 2,
            });

            context.Accounts.Add(new Account
            {
                Id = 1,
                Name = "First Bank Account for First User",
                IsUserAccount = true,
                UserId = 1,
            });
            context.Accounts.Add(new Account
            {
                Id = 2,
                Name = "First Merchant Account",
                IsUserAccount = false,
            });
            context.Accounts.Add(new Account
            {
                Id = 3,
                Name = "Second Merchant Account",
                IsUserAccount = false,
            });
            context.Accounts.Add(new Account
            {
                Id = 4,
                Name = "First Account for Second User",
                IsUserAccount = true,
                UserId = 2,
            });

            context.Transactions.Add(new Transaction
            {
                Id = 1,
                FromAccountId = 1,
                ToAccountId = 2,
                Date = DateTime.Now,
                Items = new List<TransactionItem>
                {
                    new TransactionItem
                    {
                        Id = 1,
                        BudgetId = 1,
                    },
                    new TransactionItem
                    {
                        Id = 2,
                        BudgetId = 2,
                    },
                },
                IsProcessed = false,
                Notes = "Notes for the first transaction for first user.",
            });
            context.Transactions.Add(new Transaction
            {
                Id = 2,
                FromAccountId = 1,
                ToAccountId = 3,
                Date = DateTime.Now,
                Items = new List<TransactionItem>
                {
                    new TransactionItem
                    {
                        Id = 3,
                        BudgetId = 1,
                    },
                    new TransactionItem
                    {
                        Id = 4,
                        BudgetId = 3,
                    },
                },
                IsProcessed = false,
                Notes = "Notes for the second transaction for first user.",
            });
            context.Transactions.Add(new Transaction
            {
                Id = 3,
                FromAccountId = 4,
                ToAccountId = 3,
                Date = DateTime.Now,
                Items = new List<TransactionItem>
                {
                    new TransactionItem
                    {
                        Id = 5,
                        BudgetId = 1,
                    },
                },
                IsProcessed = false,
                Notes = "Notes for the first transaction for the second user.",
            });

            context.SaveChanges();
        }
    }
}
