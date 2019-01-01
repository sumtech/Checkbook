// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Tests.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the <see cref="BudgetSummary"/> class.
    /// </summary>
    [TestClass]
    public class BudgetSummaryTests
    {
        /// <summary>
        /// Initializes the tests for the class.
        /// </summary>
        [TestInitialize]
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Cleanup the tests for the class.
        /// </summary>
        [TestCleanup]
        public virtual void Cleanup()
        {
        }

        /// <summary>
        /// Tests for the constructor method.
        /// </summary>
        [TestClass]
        public class ConstructorMethod : BudgetSummaryTests
        {
            /// <summary>
            /// The budget used as an input.
            /// </summary>
            private Budget budget;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Create the budget instance to be entered.
                this.budget = new Budget
                {
                    Id = 7,
                    Name = "Cool Budget",
                    CategoryId = 3,
                    Category = new Category
                    {
                        Id = 3,
                        Name = "Cool Category",
                        Budgets = new List<Budget>(),
                    },
                    TransactionItems = new List<TransactionItem>
                    {
                        new TransactionItem
                        {
                            Id = 1,
                            Amount = 10,
                        },
                        new TransactionItem
                        {
                            Id = 2,
                            Amount = 20,
                        },
                    },
                    UserId = 4,
                    User = new User { Id = 4 },
                };
                this.budget.Category.Budgets.Add(this.budget);
            }

            /// <summary>
            /// Verifies the budget properties get copied.
            /// </summary>
            [TestMethod]
            public void CopiesMainProperties()
            {
                // Act.
                BudgetSummary actual = new BudgetSummary(this.budget);

                // Assert.
                Assert.AreEqual(this.budget.Id, actual.Id, "The values for ID should match.");
                Assert.AreEqual(this.budget.Name, actual.Name, "The values for Name should match.");
                Assert.AreEqual(this.budget.CategoryId, actual.CategoryId, "The values for CategoryId should match.");
                Assert.AreEqual(this.budget.Category, actual.Category, "The values for Category should match.");
                Assert.AreEqual(this.budget.UserId, actual.UserId, "The values for UserId should match.");
                Assert.AreEqual(this.budget.User, actual.User, "The values for User should match.");
            }

            /// <summary>
            /// Verifies the category list of budgets is cleared, which prevents returning unwanted data such as transaction items.
            /// </summary>
            [TestMethod]
            public void ClearsCategoryBudgets()
            {
                // Act.
                BudgetSummary actual = new BudgetSummary(this.budget);

                // Assert.
                Assert.IsNotNull(actual.Category.Budgets, "The list of budgets for the category should not be null.");
                Assert.AreEqual(0, actual.Category.Budgets.Count(), "The list of budgets for the category should be empty.");
            }

            /// <summary>
            /// Verifies the balance gets calculated.
            /// </summary>
            [TestMethod]
            public void CalculatesBalance()
            {
                // Act.
                BudgetSummary actual = new BudgetSummary(this.budget);

                // Assert.
                decimal expected = this.budget.TransactionItems.Sum(ti => ti.Amount);
                Assert.AreEqual(expected, actual.Balance, "The value for the balance should be correct.");
            }

            /// <summary>
            /// Verifies zero balance when no transaction items.
            /// </summary>
            [TestMethod]
            public void SetsBalanceToZeroWhenNoTransactionItems()
            {
                // Arrange.
                this.budget.TransactionItems = null;

                // Act.
                BudgetSummary actual = new BudgetSummary(this.budget);

                // Assert.
                Assert.AreEqual(0, actual.Balance, "The value for the balance should be zero.");
            }
        }
    }
}
