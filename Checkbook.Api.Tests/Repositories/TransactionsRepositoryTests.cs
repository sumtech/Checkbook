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
    /// Tests for the <see cref="TransactionsRepository"/> class.
    /// </summary>
    [TestClass]
    public class TransactionsRepositoryTests
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
        public class GetAllMethod : TransactionsRepositoryTests
        {
            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Transaction> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the input.
                this.userId = 1;

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetTransactions(context)
                        .Where(t => t.FromAccount.UserId == this.userId || t.ToAccount.UserId == this.userId)
                        .ToList();
                }
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsTransactionsFromContext()
            {
                // Act.
                List<Transaction> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    actualList = repository.GetAll(this.userId).ToList();
                }

                // Assert.
                Assert.AreEqual(this.entities.Count(), actualList.Count(), "The entity count should match.");

                Transaction expected = this.entities.ElementAt(0);
                Transaction actual = actualList.ElementAt(0);
                string index = "first";
                Assert.AreEqual(expected.Id, actual.Id, $"The ID for the {index} entity should match.");
                Assert.IsNotNull(actual.FromAccount, $"The from account for the {index} entity should be returned.");
                Assert.AreEqual(expected.FromAccount.Id, actual.FromAccount.Id, $"The from account for the {index} entity should match.");
                Assert.IsNotNull(actual.ToAccount, $"The to account for the {index} entity should be returned.");
                Assert.AreEqual(expected.ToAccount.Id, actual.ToAccount.Id, $"The to account for the {index} entity should match.");
                Assert.IsNotNull(actual.Items, $"The items for the {index} entity should be returned.");
                Assert.AreEqual(expected.Items.Count(), actual.Items.Count(), $"The number of items from the {index} transaction should match.");
                Assert.IsNotNull(actual.Items.ElementAt(0).Budget, $"The budget for the first item of the first {index} should be present.");
                Assert.AreEqual(expected.Items.ElementAt(0).Budget.Id, actual.Items.ElementAt(0).Budget.Id, $"The ID for the budget for the first item of the {index} transaction should match.");
                Assert.IsNotNull(actual.Items.ElementAt(0).Budget.Category, $"The category for the first item of the {index} transaction should be present.");
                Assert.AreEqual(expected.Items.ElementAt(0).Budget.Category.Id, actual.Items.ElementAt(0).Budget.Category.Id, $"The ID for the category for the first item of the {index} transaction should match.");

                expected = this.entities.ElementAt(1);
                actual = actualList.ElementAt(1);
                index = "second";
                Assert.AreEqual(expected.Id, actual.Id, $"The ID for the {index} entity should match.");
                Assert.IsNotNull(actual.FromAccount, $"The from account for the {index} entity should be returned.");
                Assert.AreEqual(expected.FromAccount.Id, actual.FromAccount.Id, $"The from account for the {index} entity should match.");
                Assert.IsNotNull(actual.ToAccount, $"The to account for the {index} entity should be returned.");
                Assert.AreEqual(expected.ToAccount.Id, actual.ToAccount.Id, $"The to account for the {index} entity should match.");
                Assert.IsNotNull(actual.Items, $"The items for the {index} entity should be returned.");
                Assert.AreEqual(expected.Items.Count(), actual.Items.Count(), $"The number of items from the {index} transaction should match.");
                Assert.IsNotNull(actual.Items.ElementAt(0).Budget, $"The budget for the first item of the {index} transaction should be present.");
                Assert.AreEqual(expected.Items.ElementAt(0).Budget.Id, actual.Items.ElementAt(0).Budget.Id, $"The ID for the budget for the first item of the {index} transaction should match.");
                Assert.IsNotNull(actual.Items.ElementAt(0).Budget.Category, $"The category for the first item of the {index} transaction should be present.");
                Assert.AreEqual(expected.Items.ElementAt(0).Budget.Category.Id, actual.Items.ElementAt(0).Budget.Category.Id, $"The ID for the category for the first item of the {index} transaction should match.");
            }

            /// <summary>
            /// Verifies only records for the user are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsTransactionsOnlyForUser()
            {
                // Act.
                List<Transaction> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    actualList = repository.GetAll(this.userId).ToList();
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    List<long> userIds = context.Transactions
                        .Where(t => t.FromAccount.UserId != null)
                        .Select(t => t.FromAccount.UserId ?? 0)
                        .ToList();

                    userIds.AddRange(context.Transactions
                        .Where(t => t.ToAccount.UserId != null)
                        .Select(t => t.ToAccount.UserId ?? 0)
                        .ToList());

                    int numberOfTransactionUsers = userIds.Distinct().Count();

                    Assert.AreNotEqual(numberOfTransactionUsers, 0, "For the test to work there must be more than one user with transactions.");
                    Assert.AreNotEqual(numberOfTransactionUsers, 1, "For the test to work there must be more than one user with transactions.");
                }

                foreach (Transaction actual in actualList)
                {
                    if (actual.FromAccount.UserId != null && actual.FromAccount.UserId != this.userId)
                    {
                        Assert.Fail("The from account should have a null or matching user ID.");
                    }

                    if (actual.ToAccount.UserId != null && actual.ToAccount.UserId != this.userId)
                    {
                        Assert.Fail("The to account should have a null or matching user ID.");
                    }
                }
            }

            /// <summary>
            /// Verifies an empty list is returned when no records are found.
            /// </summary>
            [TestMethod]
            public void ReturnsEmptyListWhenNoRecordsFoundForUser()
            {
                // Arrange.
                this.userId = 123;

                // Act.
                List<Transaction> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    actualList = repository.GetAll(this.userId).ToList();
                }

                // Assert.
                Assert.AreEqual(0, actualList.Count(), "An empty list should be returned.");
            }
        }

        /// <summary>
        /// Tests for the Get(id) method.
        /// </summary>
        [TestClass]
        public class GetMethod : TransactionsRepositoryTests
        {
            /// <summary>
            /// The transaction ID used as an input.
            /// </summary>
            private long transactionId;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Transaction> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the input.
                this.transactionId = 2;
                this.userId = 1;

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetTransactions(context);
                }
            }

            /// <summary>
            /// Verifies the correct entity from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsTransactionFromContext()
            {
                // Act.
                Transaction actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    actual = repository.Get(this.transactionId, this.userId);
                }

                // Assert.
                Transaction expected = this.entities.ElementAt(1);
                Assert.AreEqual(expected.Id, actual.Id, "The ID for the entity should match.");
                Assert.IsNotNull(actual.FromAccount, "The from account for the entity should be returned.");
                Assert.AreEqual(expected.FromAccount.Id, actual.FromAccount.Id, "The from account for the entity should match.");
                Assert.IsNotNull(actual.ToAccount, "The to account for the entity should be returned.");
                Assert.AreEqual(expected.ToAccount.Id, actual.ToAccount.Id, "The to account for the entity should match.");
                Assert.IsNotNull(actual.Items, "The items for the entity should be returned.");
                Assert.AreEqual(expected.Items.Count(), actual.Items.Count(), "The number of items from the transaction should match.");
                Assert.IsNotNull(actual.Items.ElementAt(0).Budget, "The budget for the first item of the transaction should be present.");
                Assert.AreEqual(expected.Items.ElementAt(0).Budget.Id, actual.Items.ElementAt(0).Budget.Id, "The ID for the budget for the first item of the transaction should match.");
                Assert.IsNotNull(actual.Items.ElementAt(0).Budget.Category, "The category for the first item of the transaction should be present.");
                Assert.AreEqual(expected.Items.ElementAt(0).Budget.Category.Id, actual.Items.ElementAt(0).Budget.Category.Id, "The ID for the category for the first item of the transaction should match.");
            }

            /// <summary>
            /// Verifies exception thrown when no record found.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNoRecordFoundForTheUser()
            {
                // Arrange.
                this.transactionId = 123;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Get(this.transactionId, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(NotFoundException), "This should be an argument exception.");
                string exceptionMessage = "The transaction was not found.";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies only records for the user are returned.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenRecordFoundDoesNotBelongToTheUser()
            {
                // Arrange.
                this.transactionId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Get(this.transactionId, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(NotFoundException), "This should be an argument exception.");
                string exceptionMessage = "The transaction was not found.";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }

        /// <summary>
        /// Tests for the Add(transaction) method.
        /// </summary>
        [TestClass]
        public class AddMethod : TransactionsRepositoryTests
        {
            /// <summary>
            /// The transaction used as an input.
            /// </summary>
            private Transaction transaction;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Transaction> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the input.
                this.transaction = new Transaction
                {
                    FromAccountId = 1,
                    ToAccountId = 2,
                    UserId = 1,
                    Date = DateTime.Now,
                    Items = new List<TransactionItem>
                    {
                        new TransactionItem
                        {
                            BudgetId = 1,
                            Amount = 100,
                        },
                        new TransactionItem
                        {
                            BudgetId = 2,
                            Amount = 200,
                        },
                    },
                    IsProcessed = false,
                    Notes = "Notes for the first transaction.",
                };

                this.userId = 1;

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetTransactions(context);
                }
            }

            /// <summary>
            /// Verifies the correct entity from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsTransactionFromContext()
            {
                // Act.
                Transaction actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    actual = repository.Add(this.transaction, this.userId);
                }

                // Assert.
                long expectedId = 4;
                Assert.AreEqual(expectedId, actual.Id, "The ID for the entity should match.");
            }

            /// <summary>
            /// Verifies the new entity is in the context.
            /// </summary>
            [TestMethod]
            public void AddsTransactionToContext()
            {
                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    repository.Add(this.transaction, this.userId);
                }

                // Assert.
                int expectedTransactionCount = 4;
                long expectedId = 4;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    Assert.AreEqual(expectedTransactionCount, context.Transactions.Count(), "There should be the correct number of transactions.");

                    Transaction actual = ContextDataService.GetTransactionsSet(context)
                        .FirstOrDefault(x => x.Id == expectedId);
                    Assert.IsNotNull(actual, "A transaction should be found.");

                    Transaction expected = this.transaction;
                    Assert.IsNotNull(actual.FromAccount, "The from account for the entity should be returned.");
                    Assert.AreEqual(expected.FromAccountId, actual.FromAccount.Id, "The from account for the entity should match.");
                    Assert.IsNotNull(actual.ToAccount, "The to account for the entity should be returned.");
                    Assert.AreEqual(expected.ToAccountId, actual.ToAccount.Id, "The to account for the entity should match.");
                    Assert.IsNotNull(actual.Items, "The items for the entity should be returned.");
                    Assert.AreEqual(expected.Items.Count(), actual.Items.Count(), "The number of items from the transaction should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(0).Budget, "The budget for the first item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(0).BudgetId, actual.Items.ElementAt(0).Budget.Id, "The ID for the budget for the first item of the transaction should match.");
                }
            }

            /// <summary>
            /// Verifies an exception is thrown when the entity being added has an ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenIdPresent()
            {
                // Arrange.
                this.transaction.Id = 5;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Add(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A new transaction without a specified ID should have been used. To update a transaction, use the Save method.\r\nParameter name: transaction.Id";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when buedget has a zero user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenTransactionWithZeroUserId()
            {
                // Arrange.
                this.transaction.UserId = 0;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Add(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A transaction is expected to have a user ID.\r\nParameter name: transaction.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when zero user ID passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenZeroUserIdPassedIn()
            {
                // Arrange.
                this.transaction.UserId = 1;
                this.userId = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Add(this.transaction, this.userId);
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
            /// Verifies exception when transaction user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenTransactionWithMismatchedUserIds()
            {
                // Arrange.
                this.transaction.UserId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Add(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to match the passed in user ID for a transaction.\r\nParameter name: transaction.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception thrown when transaction has account for another user.
            /// </summary>
            [TestMethod]
            public void ThrowExceptionWhenFromAccountForAnotherUser()
            {
                // Arrange.
                this.transaction.FromAccountId = 1;
                this.transaction.ToAccountId = 2;
                this.transaction.UserId = 2;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Add(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "The account was not found.\r\nParameter name: transaction.FromAccountId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception thrown when transaction has account for another user.
            /// </summary>
            [TestMethod]
            public void ThrowExceptionWhenToAccountForAnotherUser()
            {
                // Arrange.
                this.transaction.FromAccountId = 2;
                this.transaction.ToAccountId = 1;
                this.transaction.UserId = 2;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Add(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "The account was not found.\r\nParameter name: transaction.ToAccountId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception thrown when transaction is between two merchant accounts.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenTransactionBetweenTwoMerchantAccounts()
            {
                // Arrange.
                this.transaction.FromAccountId = 2;
                this.transaction.ToAccountId = 3;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Add(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "At least on of the accounts needs to be an account for the user.\r\nParameter name: accounts";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }

        /// <summary>
        /// Tests for the Save() method.
        /// </summary>
        [TestClass]
        public class SaveMethod : TransactionsRepositoryTests
        {
            /// <summary>
            /// The transaction used as an input.
            /// </summary>
            private Transaction transaction;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Transaction> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the input.
                this.transaction = new Transaction
                {
                    Id = 2,
                    FromAccountId = 1,
                    ToAccountId = 3,
                    UserId = 1,
                    Date = Convert.ToDateTime("1/1/2000"),
                    Items = new List<TransactionItem>
                    {
                        new TransactionItem
                        {
                            Id = 3,
                            BudgetId = 1,
                            Amount = 100,
                        },
                        new TransactionItem
                        {
                            Id = 4,
                            BudgetId = 3,
                            Amount = 200,
                        },
                    },
                    IsProcessed = true,
                    Notes = "Notes for the first transaction.",
                };

                this.userId = 1;

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetTransactions(context);
                }
            }

            /// <summary>
            /// Verifies the correct entity from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsTransactionFromContext()
            {
                // Act.
                Transaction actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    actual = repository.Save(this.transaction, this.userId);
                }

                // Assert.
                Assert.AreEqual(this.transaction.Id, actual.Id, "The ID for the entity should match.");
            }

            /// <summary>
            /// Verifies the entity was updated in the context.
            /// </summary>
            [TestMethod]
            public void UpdatesTransactionInTheContext()
            {
                // Arrange.
                this.transaction.Items = new List<TransactionItem>
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
                };

                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    repository.Save(this.transaction, this.userId);
                }

                // Assert.
                int expectedTransactionCount = 3;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    Assert.AreEqual(expectedTransactionCount, context.Transactions.Count(), "The number of transactions should not change.");

                    Transaction actual = ContextDataService.GetTransactionsSet(context)
                        .FirstOrDefault(x => x.Id == this.transaction.Id);
                    Assert.IsNotNull(actual, "A transaction should be found.");

                    Transaction expected = this.transaction;
                    Assert.AreEqual(expected.Date, actual.Date, "The date should be updated.");
                    Assert.AreEqual(expected.IsProcessed, actual.IsProcessed, "The processed flag should be updated.");
                    Assert.IsNotNull(actual.FromAccount, "The from account for the entity should be returned.");
                    Assert.AreEqual(expected.FromAccountId, actual.FromAccount.Id, "The from account for the entity should match.");
                    Assert.IsNotNull(actual.ToAccount, "The to account for the entity should be returned.");
                    Assert.AreEqual(expected.ToAccountId, actual.ToAccount.Id, "The to account for the entity should match.");
                    Assert.IsNotNull(actual.Items, "The items for the entity should be returned.");
                    Assert.AreEqual(expected.Items.Count(), actual.Items.Count(), "The number of items from the transaction should match.");
                }
            }

            /// <summary>
            /// Verifies items get updated in the context.
            /// </summary>
            [TestMethod]
            public void UpdatesTransactionItemsInTheContext()
            {
                // Arrange.
                this.transaction.Items = new List<TransactionItem>
                {
                    new TransactionItem
                    {
                        Id = 3,
                        BudgetId = 2,
                        Amount = 100,
                    },
                    new TransactionItem
                    {
                        Id = 4,
                        BudgetId = 1,
                        Amount = 200,
                    },
                };

                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    repository.Save(this.transaction, this.userId);
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    Transaction actual = ContextDataService.GetTransactionsSet(context)
                        .FirstOrDefault(x => x.Id == this.transaction.Id);
                    Assert.IsNotNull(actual, "A transaction should be found.");

                    Transaction expected = this.transaction;
                    Assert.IsNotNull(actual.Items, "The items for the entity should be returned.");
                    Assert.AreEqual(expected.Items.Count(), actual.Items.Count(), "The number of items from the transaction should match.");

                    Assert.AreEqual(expected.Items.ElementAt(0).Id, actual.Items.ElementAt(0).Id, "The ID for the first item should match.");
                    Assert.AreEqual(expected.Items.ElementAt(0).Amount, actual.Items.ElementAt(0).Amount, "The amount for the first item should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(0).Budget, "The budget for the first item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(0).BudgetId, actual.Items.ElementAt(0).Budget.Id, "The ID for the budget for the first item of the transaction should match.");

                    Assert.AreEqual(expected.Items.ElementAt(1).Id, actual.Items.ElementAt(1).Id, "The ID for the second item should match.");
                    Assert.AreEqual(expected.Items.ElementAt(1).Amount, actual.Items.ElementAt(1).Amount, "The amount for the second item should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(1).Budget, "The budget for the second item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(1).BudgetId, actual.Items.ElementAt(1).Budget.Id, "The ID for the budget for the second item of the transaction should match.");
                }
            }

            /// <summary>
            /// Verifies items get added to the context.
            /// </summary>
            [TestMethod]
            public void AddsTransactionItemToTheContext()
            {
                // Arrange.
                this.transaction.Items.Add(new TransactionItem
                {
                    BudgetId = 1,
                    Amount = 300,
                });

                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    repository.Save(this.transaction, this.userId);
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    Transaction actual = ContextDataService.GetTransactionsSet(context)
                        .FirstOrDefault(x => x.Id == this.transaction.Id);
                    Assert.IsNotNull(actual, "A transaction should be found.");

                    Transaction expected = this.transaction;
                    Assert.IsNotNull(actual.Items, "The items for the entity should be returned.");
                    Assert.AreEqual(3, actual.Items.Count(), "The number of items should be increased by 1.");

                    Assert.AreEqual(expected.Items.ElementAt(0).Id, actual.Items.ElementAt(0).Id, "The ID for the first item should match.");
                    Assert.AreEqual(expected.Items.ElementAt(0).Amount, actual.Items.ElementAt(0).Amount, "The amount for the first item should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(0).Budget, "The budget for the first item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(0).BudgetId, actual.Items.ElementAt(0).Budget.Id, "The ID for the budget for the first item of the transaction should match.");

                    Assert.AreEqual(expected.Items.ElementAt(1).Id, actual.Items.ElementAt(1).Id, "The ID for the second item should match.");
                    Assert.AreEqual(expected.Items.ElementAt(1).Amount, actual.Items.ElementAt(1).Amount, "The amount for the second item should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(1).Budget, "The budget for the second item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(1).BudgetId, actual.Items.ElementAt(1).Budget.Id, "The ID for the budget for the second item of the transaction should match.");

                    Assert.AreEqual(expected.Items.ElementAt(2).Id, actual.Items.ElementAt(2).Id, "The ID for the third item should match.");
                    Assert.AreEqual(expected.Items.ElementAt(2).Amount, actual.Items.ElementAt(2).Amount, "The amount for the third item should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(2).Budget, "The budget for the third item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(2).BudgetId, actual.Items.ElementAt(2).Budget.Id, "The ID for the budget for the third item of the transaction should match.");
                }
            }

            /// <summary>
            /// Verifies items get added to the context.
            /// </summary>
            [TestMethod]
            public void AddsTransactionItemsToTheContext()
            {
                // Arrange.
                this.transaction.Items.Add(new TransactionItem
                {
                    BudgetId = 1,
                    Amount = 300,
                });
                this.transaction.Items.Add(new TransactionItem
                {
                    BudgetId = 1,
                    Amount = 400,
                });

                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    repository.Save(this.transaction, this.userId);
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    Transaction actual = ContextDataService.GetTransactionsSet(context)
                        .FirstOrDefault(x => x.Id == this.transaction.Id);
                    Assert.IsNotNull(actual, "A transaction should be found.");

                    Transaction expected = this.transaction;
                    Assert.IsNotNull(actual.Items, "The items for the entity should be returned.");
                    Assert.AreEqual(4, actual.Items.Count(), "The number of items should be increased by 2.");

                    Assert.AreEqual(expected.Items.ElementAt(0).Id, actual.Items.ElementAt(0).Id, "The ID for the first item should match.");
                    Assert.AreEqual(expected.Items.ElementAt(0).Amount, actual.Items.ElementAt(0).Amount, "The amount for the first item should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(0).Budget, "The budget for the first item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(0).BudgetId, actual.Items.ElementAt(0).Budget.Id, "The ID for the budget for the first item of the transaction should match.");

                    Assert.AreEqual(expected.Items.ElementAt(1).Id, actual.Items.ElementAt(1).Id, "The ID for the second item should match.");
                    Assert.AreEqual(expected.Items.ElementAt(1).Amount, actual.Items.ElementAt(1).Amount, "The amount for the second item should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(1).Budget, "The budget for the second item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(1).BudgetId, actual.Items.ElementAt(1).Budget.Id, "The ID for the budget for the second item of the transaction should match.");

                    Assert.AreEqual(expected.Items.ElementAt(2).Id, actual.Items.ElementAt(2).Id, "The ID for the third item should match.");
                    Assert.AreEqual(expected.Items.ElementAt(2).Amount, actual.Items.ElementAt(2).Amount, "The amount for the third item should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(2).Budget, "The budget for the third item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(2).BudgetId, actual.Items.ElementAt(2).Budget.Id, "The ID for the budget for the third item of the transaction should match.");

                    Assert.AreEqual(expected.Items.ElementAt(3).Id, actual.Items.ElementAt(3).Id, "The ID for the fourth item should match.");
                    Assert.AreEqual(expected.Items.ElementAt(3).Amount, actual.Items.ElementAt(3).Amount, "The amount for the fourth item should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(3).Budget, "The budget for the fourth item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(3).BudgetId, actual.Items.ElementAt(3).Budget.Id, "The ID for the budget for the fourth item of the transaction should match.");
                }
            }

            /// <summary>
            /// Verifies items get removed form the context.
            /// </summary>
            [TestMethod]
            public void RemovesTransactionItemsFromTheContext()
            {
                // Arrange.
                this.transaction.Items.RemoveAt(0);

                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    TransactionsRepository repository = new TransactionsRepository(context);
                    repository.Save(this.transaction, this.userId);
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    Transaction actual = ContextDataService.GetTransactionsSet(context)
                        .FirstOrDefault(x => x.Id == this.transaction.Id);
                    Assert.IsNotNull(actual, "A transaction should be found.");

                    Transaction expected = this.transaction;
                    Assert.IsNotNull(actual.Items, "The items for the entity should be returned.");
                    Assert.AreEqual(1, actual.Items.Count(), "The number of items should be reduced by 1.");

                    Assert.AreEqual(expected.Items.ElementAt(0).Id, actual.Items.ElementAt(0).Id, "The ID for the first item should match.");
                    Assert.AreEqual(expected.Items.ElementAt(0).Amount, actual.Items.ElementAt(0).Amount, "The amount for the first item should match.");
                    Assert.IsNotNull(actual.Items.ElementAt(0).Budget, "The budget for the first item of the transaction should be present.");
                    Assert.AreEqual(expected.Items.ElementAt(0).BudgetId, actual.Items.ElementAt(0).Budget.Id, "The ID for the budget for the first item of the transaction should match.");
                }
            }

            /// <summary>
            /// Verifies an exception is thrown when the entity being added has an ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenIdZero()
            {
                // Arrange.
                this.transaction.Id = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Save(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A new transaction with a specified ID should have been used. To add a transaction, use the Add method.\r\nParameter name: transaction.Id";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when transaction has a zero user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenTransactionWithZeroUserId()
            {
                // Arrange.
                this.transaction.UserId = 0;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Save(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A transaction is expected to have a user ID.\r\nParameter name: transaction.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when null user ID passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenZeroUserIdPasedIn()
            {
                // Arrange.
                this.transaction.UserId = 1;
                this.userId = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Save(this.transaction, this.userId);
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
            /// Verifies exception when transaction user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenTransactionWithMismatchedUserIds()
            {
                // Arrange.
                this.transaction.UserId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Save(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to match the passed in user ID for a transaction.\r\nParameter name: transaction.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception thrown when transaction has account for another user.
            /// </summary>
            [TestMethod]
            public void ThrowExceptionWhenFromAccountForAnotherUser()
            {
                // Arrange.
                this.transaction.FromAccountId = 1;
                this.transaction.ToAccountId = 2;
                this.transaction.UserId = 2;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Save(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "The account was not found.\r\nParameter name: transaction.FromAccountId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception thrown when transaction has account for another user.
            /// </summary>
            [TestMethod]
            public void ThrowExceptionWhenToAccountForAnotherUser()
            {
                // Arrange.
                this.transaction.FromAccountId = 2;
                this.transaction.ToAccountId = 1;
                this.transaction.UserId = 2;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Save(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "The account was not found.\r\nParameter name: transaction.ToAccountId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception thrown when transaction is between two merchant accounts.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenTransactionBetweenTwoMerchantAccounts()
            {
                // Arrange.
                this.transaction.FromAccountId = 2;
                this.transaction.ToAccountId = 3;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Save(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "At least on of the accounts needs to be an account for the user.\r\nParameter name: accounts";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception thrown when transaction previously belonged to another user.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenPreviousTransactionForAnotherUser()
            {
                // Arrange.
                this.transaction.FromAccountId = 4;
                this.transaction.UserId = 2;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        TransactionsRepository repository = new TransactionsRepository(context);
                        repository.Save(this.transaction, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "This transaction belongs to another user.\r\nParameter name: accounts";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }
    }
}
