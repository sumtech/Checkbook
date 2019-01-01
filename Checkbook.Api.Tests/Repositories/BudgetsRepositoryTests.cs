// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Checkbook.Api.Tests.Helpers;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests for the <see cref="BudgetsRepository"/> class.
    /// </summary>
    [TestClass]
    public class BudgetsRepositoryTests
    {
        /// <summary>
        /// The options for the test implementation of the database context.
        /// </summary>
        private DbContextOptions<CheckbookContext> dbContextOptions;

        /// <summary>
        /// The database connection used for a group of tests.
        /// </summary>
        private DbConnection dbConnection;

        /// <summary>
        /// Initializes the tests for the class.
        /// </summary>
        [TestInitialize]
        public virtual void Initialize()
        {
            // Open a connection to a SQLite database.
            string connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = ":memory:",
            }.ToString();
            this.dbConnection = new SqliteConnection(connectionString);
            this.dbConnection.Open();

            // Create a new context.
            DbContextOptionsBuilder<CheckbookContext> builder = new DbContextOptionsBuilder<CheckbookContext>();
            builder.UseSqlite(this.dbConnection);
            this.dbContextOptions = builder.Options;

            // Make sure the database has been created.
            using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
            {
                context.Database.EnsureCreated();
            }
        }

        /// <summary>
        /// Cleanup the tests for the class.
        /// </summary>
        [TestCleanup]
        public virtual void Cleanup()
        {
            this.dbConnection.Close();
        }

        /// <summary>
        /// Tests for the GetAll() method.
        /// </summary>
        [TestClass]
        public class GetAllMethod : BudgetsRepositoryTests
        {
            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Budget> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the inputs.
                this.userId = 1;

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetBudgets(context)
                        .Where(a => a.UserId == this.userId)
                        .ToList();
                }
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBudgetsFromContext()
            {
                // Act.
                List<Budget> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    actualList = repository.GetAll(this.userId).ToList();
                }

                // Assert.
                Assert.AreEqual(this.entities.Count(), actualList.Count(), "The entity count should match.");

                Budget expected = this.entities.ElementAt(0);
                Budget actual = actualList.ElementAt(0);
                string index = "first";
                Assert.AreEqual(expected.Id, actual.Id, $"The ID for the {index} entity should match.");
                Assert.AreEqual(expected.Name, actual.Name, $"The name for the {index} entity should match.");
                Assert.AreEqual(expected.UserId, actual.UserId, $"The user ID for the {index} entity should match.");
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBudgetsOnlyForTheUser()
            {
                // Act.
                List<Budget> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    actualList = repository.GetAll(this.userId).ToList();
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    int numberOfBudgetUsers = context.Budgets
                        .GroupBy(a => a.UserId)
                        .Count();
                    Assert.AreNotEqual(numberOfBudgetUsers, 0, "For the test to work there must be more than one user with budgets.");
                    Assert.AreNotEqual(numberOfBudgetUsers, 1, "For the test to work there must be more than one user with budgets.");
                }

                foreach (Budget actual in actualList)
                {
                    Assert.AreEqual(this.userId, actual.UserId, "The user ID for each budget should match the input user ID.");
                }
            }

            /// <summary>
            /// Verifies an empty list is returned when no records are found.
            /// </summary>
            [TestMethod]
            public void ReturnsEmptyListWhenNoRecordsFound()
            {
                // Arrange.
                this.userId = 123;

                // Act.
                List<Budget> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    actualList = repository.GetAll(this.userId).ToList();
                }

                // Assert.
                Assert.AreEqual(0, actualList.Count(), "An empty list should be returned.");
            }
        }

        /// <summary>
        /// Tests for the GetTotals() method.
        /// </summary>
        [TestClass]
        public class GetTotalsMethod : BudgetsRepositoryTests
        {
            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Budget> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the inputs.
                this.userId = 1;

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetBudgets(context)
                        .Where(a => a.UserId == this.userId)
                        .ToList();
                }
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBudgetsFromContext()
            {
                // Act.
                List<BudgetSummary> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    actualList = repository.GetTotals(this.userId).ToList();
                }

                // Assert.
                Assert.AreEqual(this.entities.Count(), actualList.Count(), "The entity count should match.");

                BudgetSummary expected = new BudgetSummary(this.entities.ElementAt(0));
                BudgetSummary actual = actualList.ElementAt(0);
                string index = "first";
                Assert.AreEqual(expected.Id, actual.Id, $"The ID for the {index} entity should match.");
                Assert.AreEqual(expected.Name, actual.Name, $"The name for the {index} entity should match.");
                Assert.AreEqual(expected.UserId, actual.UserId, $"The user ID for the {index} entity should match.");
            }

            /// <summary>
            /// Verifies the totals from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBudgetTotalsFromContext()
            {
                // Act.
                List<BudgetSummary> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    actualList = repository.GetTotals(this.userId).ToList();
                }

                // Assert.
                Assert.AreEqual(this.entities.Count(), actualList.Count(), "The entity count should match.");

                BudgetSummary expected = new BudgetSummary(this.entities.ElementAt(0));
                BudgetSummary actual = actualList.ElementAt(0);
                string index = "first";
                Assert.AreNotEqual(0, actual.Balance, "The current total should not be zero.");
                Assert.AreEqual(expected.Balance, actual.Balance, $"The current total for the {index} entity should match.");
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBudgetsOnlyForTheUser()
            {
                // Act.
                List<BudgetSummary> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    actualList = repository.GetTotals(this.userId).ToList();
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    int numberOfBudgetUsers = context.Budgets
                        .GroupBy(a => a.UserId)
                        .Count();
                    Assert.AreNotEqual(numberOfBudgetUsers, 0, "For the test to work there must be more than one user with budgets.");
                    Assert.AreNotEqual(numberOfBudgetUsers, 1, "For the test to work there must be more than one user with budgets.");
                }

                foreach (BudgetSummary actual in actualList)
                {
                    Assert.AreEqual(this.userId, actual.UserId, "The user ID for each budget should match the input user ID.");
                }
            }

            /// <summary>
            /// Verifies an empty list is returned when no records are found.
            /// </summary>
            [TestMethod]
            public void ReturnsEmptyListWhenNoRecordsFound()
            {
                // Arrange.
                this.userId = 123;

                // Act.
                List<BudgetSummary> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    actualList = repository.GetTotals(this.userId).ToList();
                }

                // Assert.
                Assert.AreEqual(0, actualList.Count(), "An empty list should be returned.");
            }
        }

        /// <summary>
        /// Tests for the Get(id) method.
        /// </summary>
        [TestClass]
        public class GetMethod : BudgetsRepositoryTests
        {
            /// <summary>
            /// The budget ID used as an input.
            /// </summary>
            private long budgetId;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Budget> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetBudgets(context);
                }
            }

            /// <summary>
            /// Verifies the correct budget from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBudgetFromContext()
            {
                // Arrange.
                this.budgetId = 1;
                this.userId = 1;

                // Act.
                Budget actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    actual = repository.Get(this.budgetId, this.userId);
                }

                // Assert.
                Budget expected = this.entities.ElementAt(0);
                Assert.AreEqual(expected.Id, actual.Id, "The ID for the entity should match.");
                Assert.AreEqual(expected.Name, actual.Name, "The name for the entity should match.");
                Assert.AreEqual(expected.UserId, actual.UserId, "The user ID for the entity should match.");
            }

            /// <summary>
            /// Verifies exception thrown when no record found.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNoRecordFoundForTheUser()
            {
                // Arrange.
                this.budgetId = 123;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        BudgetsRepository repository = new BudgetsRepository(context);
                        repository.Get(this.budgetId, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(NotFoundException), "This should be an argument exception.");
                string exceptionMessage = "The budget was not found.";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when budget user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenRecordFoundDoesNotBelongToTheUser()
            {
                // Arrange.
                this.budgetId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        BudgetsRepository repository = new BudgetsRepository(context);
                        repository.Get(this.budgetId, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(NotFoundException), "This should be an argument exception.");
                string exceptionMessage = "The budget was not found.";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }

        /// <summary>
        /// Tests for the Add(budget) method.
        /// </summary>
        [TestClass]
        public class AddMethod : BudgetsRepositoryTests
        {
            /// <summary>
            /// The budget used as an input.
            /// </summary>
            private Budget budget;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Budget> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the inputs.
                this.userId = 1;
                this.budget = new Budget
                {
                    Name = "Third Budget",
                    UserId = this.userId,
                    CategoryId = 1,
                };

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetBudgets(context);
                }
            }

            /// <summary>
            /// Verifies the correct entity from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBudgetFromContext()
            {
                // Act.
                Budget actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    actual = repository.Add(this.budget, this.userId);
                }

                // Assert.
                long expectedId = 6;
                Assert.AreEqual(expectedId, actual.Id, "The ID for the entity should match.");
            }

            /// <summary>
            /// Verifies the new entity is in the context.
            /// </summary>
            [TestMethod]
            public void AddsBudgetToContext()
            {
                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    repository.Add(this.budget, this.userId);
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    int expectedBudgetCount = 6;
                    Assert.AreEqual(expectedBudgetCount, context.Budgets.Count(), "There should be the correct number of budgets.");

                    long expectedId = 6;
                    Budget actual = ContextDataService.GetBudgetsSet(context)
                        .FirstOrDefault(x => x.Id == expectedId);
                    Assert.IsNotNull(actual, "A budget should be found.");

                    Budget expected = this.budget;
                    Assert.AreEqual(expected.Id, actual.Id, "The ID for the entity should match.");
                    Assert.AreEqual(expected.Name, actual.Name, "The name for the entity should match.");
                    Assert.AreEqual(expected.UserId, actual.UserId, "The user ID for the entity should match.");
                }
            }

            /// <summary>
            /// Verifies an exception is thrown when the entity being added has an ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenIdPresent()
            {
                // Arrange.
                this.budget.Id = 5;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        BudgetsRepository repository = new BudgetsRepository(context);
                        repository.Add(this.budget, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A new budget without a specified ID should have been used. To update a budget, use the Save method.\r\nParameter name: budget.Id";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when buedget has a zero user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBudgetWithZeroUserId()
            {
                // Arrange.
                this.budget.UserId = 0;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        BudgetsRepository repository = new BudgetsRepository(context);
                        repository.Add(this.budget, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A budget is expected to have a user ID.\r\nParameter name: budget.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when zero user ID passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenZeroUserIdPassedIn()
            {
                // Arrange.
                this.budget.UserId = 1;
                this.userId = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        BudgetsRepository repository = new BudgetsRepository(context);
                        repository.Add(this.budget, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to be passed in.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when budget user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenBudgetWithMismatchedUserIds()
            {
                // Arrange.
                this.budget.UserId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        BudgetsRepository repository = new BudgetsRepository(context);
                        repository.Add(this.budget, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to match the passed in user ID for a budget.\r\nParameter name: budget.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }

        /// <summary>
        /// Tests for the Save() method.
        /// </summary>
        [TestClass]
        public class SaveMethod : BudgetsRepositoryTests
        {
            /// <summary>
            /// The budget used as an input.
            /// </summary>
            private Budget budget;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Budget> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the input.
                this.userId = 1;
                this.budget = new Budget
                {
                    Id = 2,
                    Name = "Second Budget x",
                    UserId = this.userId,
                    CategoryId = 1,
                };

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetBudgets(context);
                }
            }

            /// <summary>
            /// Verifies the correct entity from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBudgetFromContext()
            {
                // Act.
                Budget actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    actual = repository.Save(this.budget, this.userId);
                }

                // Assert.
                Assert.AreEqual(this.budget.Id, actual.Id, "The ID for the entity should match.");
            }

            /// <summary>
            /// Verifies the entity was updated in the context.
            /// </summary>
            [TestMethod]
            public void UpdatesBudgetInTheContext()
            {
                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    BudgetsRepository repository = new BudgetsRepository(context);
                    repository.Save(this.budget, this.userId);
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    int expectedBudgetCount = 5;
                    Assert.AreEqual(expectedBudgetCount, context.Budgets.Count(), "The number of budgets should not change.");

                    Budget actual = ContextDataService.GetBudgetsSet(context)
                        .FirstOrDefault(x => x.Id == this.budget.Id);
                    Assert.IsNotNull(actual, "A budget should be found.");

                    Budget expected = this.budget;
                    Assert.AreEqual(expected.Id, actual.Id, "The ID for the entity should match.");
                    Assert.AreEqual(expected.Name, actual.Name, "The name for the entity should match.");
                    Assert.AreEqual(expected.UserId, actual.UserId, "The user ID for the entity should match.");
                }
            }

            /// <summary>
            /// Verifies an exception is thrown when the entity being added has an ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenIdZero()
            {
                // Arrange.
                this.budget.Id = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        BudgetsRepository repository = new BudgetsRepository(context);
                        repository.Save(this.budget, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A budget with a specified ID should have been used. To add a budget, use the Add method.\r\nParameter name: budget.Id";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when budget has a zero user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenBudgetWithZeroUserId()
            {
                // Arrange.
                this.budget.UserId = 0;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        BudgetsRepository repository = new BudgetsRepository(context);
                        repository.Save(this.budget, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A budget is expected to have a user ID.\r\nParameter name: budget.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when null user ID passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenZeroUserIdPasedIn()
            {
                // Arrange.
                this.budget.UserId = 1;
                this.userId = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        BudgetsRepository repository = new BudgetsRepository(context);
                        repository.Save(this.budget, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to be passed in.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when budget user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenBudgetWithMismatchedUserIds()
            {
                // Arrange.
                this.budget.UserId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        BudgetsRepository repository = new BudgetsRepository(context);
                        repository.Save(this.budget, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to match the passed in user ID for a budget.\r\nParameter name: budget.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }
    }
}
