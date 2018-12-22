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
    /// Tests for the <see cref="AccountsRepository"/> class.
    /// </summary>
    [TestClass]
    public class AccountsRepositoryTests
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
        /// Tests for the GetBankAccounts() method.
        /// </summary>
        [TestClass]
        public class GetBankAccountsMethod : AccountsRepositoryTests
        {
            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Account> entities;

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
                    this.entities = ContextDataService.GetAccounts(context)
                        .Where(a => a.IsUserAccount)
                        .Where(a => a.UserId == this.userId)
                        .ToList();
                }
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBankAccountsFromContext()
            {
                // Act.
                List<Account> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    actualList = repository.GetBankAccounts(this.userId).ToList();
                }

                // Assert.
                Assert.AreEqual(this.entities.Count(), actualList.Count(), "The entity count should match.");

                Account expected = this.entities.ElementAt(0);
                Account actual = actualList.ElementAt(0);
                string index = "first";
                Assert.AreEqual(expected.Id, actual.Id, $"The ID for the {index} entity should match.");
                Assert.AreEqual(expected.Name, actual.Name, $"The name for the {index} entity should match.");
                Assert.AreEqual(expected.UserId, actual.UserId, $"The user ID for the {index} entity should match.");
                Assert.IsTrue(actual.IsUserAccount, $"The flag for the {index} entity should indicate a user account.");
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBankAccountsOnlyForTheUser()
            {
                // Act.
                List<Account> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    actualList = repository.GetBankAccounts(this.userId).ToList();
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    int numberOfAccountUsers = context.Accounts
                        .Where(a => a.IsUserAccount)
                        .GroupBy(a => a.UserId)
                        .Count();
                    Assert.AreNotEqual(numberOfAccountUsers, 0, "For the test to work there must be more than one user with accounts.");
                    Assert.AreNotEqual(numberOfAccountUsers, 1, "For the test to work there must be more than one user with accounts.");
                }

                foreach (Account actual in actualList)
                {
                    Assert.AreEqual(this.userId, actual.UserId, "The user ID for each account should match the input user ID.");
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
                List<Account> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    actualList = repository.GetBankAccounts(this.userId).ToList();
                }

                // Assert.
                Assert.AreEqual(0, actualList.Count(), "An empty list should be returned.");
            }
        }

        /// <summary>
        /// Tests for the GetMerchantAccounts() method.
        /// </summary>
        [TestClass]
        public class GetMerchantAccountsMethod : AccountsRepositoryTests
        {
            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Account> entities;

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
                    this.entities = ContextDataService.GetAccounts(context)
                        .Where(a => !a.IsUserAccount)
                        .ToList();
                }
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsMerchantAccountsFromContext()
            {
                // Act.
                List<Account> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    actualList = repository.GetMerchantAccounts().ToList();
                }

                // Assert.
                Assert.AreEqual(this.entities.Count(), actualList.Count(), "The entity count should match.");

                Account expected = this.entities.ElementAt(0);
                Account actual = actualList.ElementAt(0);
                string index = "first";
                Assert.AreEqual(expected.Id, actual.Id, $"The ID for the {index} entity should match.");
                Assert.AreEqual(expected.Name, actual.Name, $"The name for the {index} entity should match.");
                Assert.AreEqual(expected.UserId, actual.UserId, $"The user ID for the {index} entity should match.");
                Assert.IsFalse(actual.IsUserAccount, $"The flag for the {index} entity should indicate a user account.");

                expected = this.entities.ElementAt(1);
                actual = actualList.ElementAt(1);
                index = "second";
                Assert.AreEqual(expected.Id, actual.Id, $"The ID for the {index} entity should match.");
                Assert.AreEqual(expected.Name, actual.Name, $"The name for the {index} entity should match.");
                Assert.AreEqual(expected.UserId, actual.UserId, $"The user ID for the {index} entity should match.");
                Assert.IsFalse(actual.IsUserAccount, $"The flag for the {index} entity should indicate a user account.");
            }

            /// <summary>
            /// Verifies an empty list is returned when no records are found.
            /// </summary>
            [TestMethod]
            public void ReturnsEmptyListWhenNoRecordsFound()
            {
                // Arrange.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    foreach (Account account in context.Accounts.ToList())
                    {
                        account.IsUserAccount = true;
                    }

                    context.SaveChanges();
                }

                // Act.
                List<Account> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    actualList = repository.GetMerchantAccounts().ToList();
                }

                // Assert.
                Assert.AreEqual(0, actualList.Count(), "An empty list should be returned.");
            }
        }

        /// <summary>
        /// Tests for the Get(id) method.
        /// </summary>
        [TestClass]
        public class GetMethod : AccountsRepositoryTests
        {
            /// <summary>
            /// The account ID used as an input.
            /// </summary>
            private long accountId;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long? userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Account> entities;

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
                    this.entities = ContextDataService.GetAccounts(context);
                }
            }

            /// <summary>
            /// Verifies the correct bank account from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsBankAccountFromContext()
            {
                // Arrange.
                this.accountId = 1;
                this.userId = 1;

                // Act.
                Account actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    actual = repository.Get(this.accountId, this.userId);
                }

                // Assert.
                Account expected = this.entities.ElementAt(0);
                Assert.AreEqual(expected.Id, actual.Id, "The ID for the entity should match.");
                Assert.AreEqual(expected.Name, actual.Name, "The name for the entity should match.");
                Assert.AreEqual(expected.UserId, actual.UserId, "The user ID for the entity should match.");
                Assert.IsTrue(actual.IsUserAccount, "The flag for the entity should indicate a user account.");
            }

            /// <summary>
            /// Verifies the correct merchant account from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsMerchantAccountFromContextWithNullUserId()
            {
                // Arrange.
                this.accountId = 2;
                this.userId = null;

                // Act.
                Account actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    actual = repository.Get(this.accountId, this.userId);
                }

                // Assert.
                Account expected = this.entities.ElementAt(1);
                Assert.AreEqual(expected.Id, actual.Id, "The ID for the entity should match.");
                Assert.AreEqual(expected.Name, actual.Name, "The name for the entity should match.");
                Assert.AreEqual(expected.UserId, actual.UserId, "The user ID for the entity should match.");
                Assert.IsFalse(actual.IsUserAccount, "The flag for the entity should indicate a user account.");
            }

            /// <summary>
            /// Verifies the correct merchant account from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsMerchantAccountFromContextWithZeroUserId()
            {
                // Arrange.
                this.accountId = 2;
                this.userId = 0;

                // Act.
                Account actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    actual = repository.Get(this.accountId, this.userId);
                }

                // Assert.
                Account expected = this.entities.ElementAt(1);
                Assert.AreEqual(expected.Id, actual.Id, "The ID for the entity should match.");
                Assert.AreEqual(expected.Name, actual.Name, "The name for the entity should match.");
                Assert.AreEqual(expected.UserId, actual.UserId, "The user ID for the entity should match.");
                Assert.IsFalse(actual.IsUserAccount, "The flag for the entity should indicate a user account.");
            }

            /// <summary>
            /// Verifies exception thrown when no record found.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNoRecordFoundForTheUser()
            {
                // Arrange.
                this.accountId = 123;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Get(this.accountId, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(NotFoundException), "This should be an argument exception.");
                string exceptionMessage = "The account was not found.";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when no user ID is passed in for a bank account user.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNoUserIdPassedInForBankAccountUser()
            {
                // Arrange.
                this.accountId = 1;
                this.userId = null;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Get(this.accountId, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "An argument exception should be thrown.");
                string exceptionMessage = "A user ID is expected to be passed in for a bank account.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when bank account user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenBankAccountUserIdDoesNotMatch()
            {
                // Arrange.
                this.accountId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Get(this.accountId, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "An argument exception should be thrown.");
                string exceptionMessage = "The user ID did not match for the bank account.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when user ID passed in for a merchant account.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenUserIdPassedInForMerchantAccount()
            {
                // Arrange.
                this.accountId = 2;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Get(this.accountId, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "An argument exception should be thrown.");
                string exceptionMessage = "The user ID was not expected for a merchant account.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }

        /// <summary>
        /// Tests for the Add(account) method.
        /// </summary>
        [TestClass]
        public class AddMethod : AccountsRepositoryTests
        {
            /// <summary>
            /// The account used as an input.
            /// </summary>
            private Account account;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long? userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Account> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the inputs.
                this.userId = 1;
                this.account = new Account
                {
                    Name = "Third Account",
                    UserId = this.userId,
                    IsUserAccount = true,
                };

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetAccounts(context);
                }
            }

            /// <summary>
            /// Verifies the correct entity from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsAccountFromContext()
            {
                // Act.
                Account actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    actual = repository.Add(this.account, this.userId);
                }

                // Assert.
                long expectedId = 5;
                Assert.AreEqual(expectedId, actual.Id, "The ID for the entity should match.");
            }

            /// <summary>
            /// Verifies the new entity is in the context.
            /// </summary>
            [TestMethod]
            public void AddsAccountToContext()
            {
                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    repository.Add(this.account, this.userId);
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    int expectedAccountCount = 5;
                    Assert.AreEqual(expectedAccountCount, context.Accounts.Count(), "There should be the correct number of accounts.");

                    long expectedId = 5;
                    Account actual = ContextDataService.GetAccountsSet(context)
                        .FirstOrDefault(x => x.Id == expectedId);
                    Assert.IsNotNull(actual, "An account should be found.");

                    Account expected = this.account;
                    Assert.AreEqual(expected.Id, actual.Id, "The ID for the entity should match.");
                    Assert.AreEqual(expected.Name, actual.Name, "The name for the entity should match.");
                    Assert.AreEqual(expected.UserId, actual.UserId, "The user ID for the entity should match.");
                    Assert.AreEqual(expected.IsUserAccount, actual.IsUserAccount, "The flag for the entity should match.");
                }
            }

            /// <summary>
            /// Verifies an exception is thrown when the entity being added has an ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenIdPresent()
            {
                // Arrange.
                this.account.Id = 5;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Add(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A new account without a specified ID should have been used. To update an account, use the Save method.\r\nParameter name: account.Id";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when bank account has a null user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBankAccountWithNullUserId()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = null;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Add(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A bank account is expected to have a user ID.\r\nParameter name: account.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when bank account has a zero user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBankAccountWithZeroUserId()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 0;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Add(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A bank account is expected to have a user ID.\r\nParameter name: account.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when null user ID passed in for a bank account.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBankAccountWithNullUserIdPasedIn()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 1;
                this.userId = null;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Add(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to be passed in for a bank account.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when zero user ID passed in for a bank account.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBankAccountWithZeroUserIdPassedIn()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 1;
                this.userId = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Add(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to be passed in for a bank account.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when bank account user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBankAccountWithMismatchedUserIds()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Add(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to match the passed in user ID for a bank account.\r\nParameter name: account.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when merchant account has a user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewMerchantWithUserId()
            {
                // Arrange.
                this.account.IsUserAccount = false;
                this.account.UserId = 1;
                this.userId = null;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Add(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A merchant account is not expected to have a user ID.\r\nParameter name: account.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when user ID passed in for a merchant account.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewMerchantWithUserIdPassedIn()
            {
                // Arrange.
                this.account.IsUserAccount = false;
                this.account.UserId = null;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Add(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is not expected to be passed in for a merchant account.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }

        /// <summary>
        /// Tests for the Save() method.
        /// </summary>
        [TestClass]
        public class SaveMethod : AccountsRepositoryTests
        {
            /// <summary>
            /// The account used as an input.
            /// </summary>
            private Account account;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long? userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Account> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the input.
                this.userId = 1;
                this.account = new Account
                {
                    Id = 2,
                    Name = "Second Account x",
                    UserId = this.userId,
                    IsUserAccount = true,
                };

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetAccounts(context);
                }
            }

            /// <summary>
            /// Verifies the correct entity from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsAccountFromContext()
            {
                // Act.
                Account actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    actual = repository.Save(this.account, this.userId);
                }

                // Assert.
                Assert.AreEqual(this.account.Id, actual.Id, "The ID for the entity should match.");
            }

            /// <summary>
            /// Verifies the entity was updated in the context.
            /// </summary>
            [TestMethod]
            public void UpdatesAccountInTheContext()
            {
                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    AccountsRepository repository = new AccountsRepository(context);
                    repository.Save(this.account, this.userId);
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    int expectedAccountCount = 4;
                    Assert.AreEqual(expectedAccountCount, context.Accounts.Count(), "The number of accounts should not change.");

                    Account actual = ContextDataService.GetAccountsSet(context)
                        .FirstOrDefault(x => x.Id == this.account.Id);
                    Assert.IsNotNull(actual, "An account should be found.");

                    Account expected = this.account;
                    Assert.AreEqual(expected.Id, actual.Id, "The ID for the entity should match.");
                    Assert.AreEqual(expected.Name, actual.Name, "The name for the entity should match.");
                    Assert.AreEqual(expected.UserId, actual.UserId, "The user ID for the entity should match.");
                    Assert.AreEqual(expected.IsUserAccount, actual.IsUserAccount, "The flag for the entity should match.");
                }
            }

            /// <summary>
            /// Verifies an exception is thrown when the entity being added has an ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenIdZero()
            {
                // Arrange.
                this.account.Id = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Save(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A new account with a specified ID should have been used. To add an account, use the Add method.\r\nParameter name: account.Id";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when bank account has a null user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBankAccountWithNullUserId()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = null;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Save(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A bank account is expected to have a user ID.\r\nParameter name: account.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when bank account has a zero user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBankAccountWithZeroUserId()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 0;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Save(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A bank account is expected to have a user ID.\r\nParameter name: account.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when null user ID passed in for a bank account.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBankAccountWithNullUserIdPasedIn()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 1;
                this.userId = null;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Save(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to be passed in for a bank account.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when zero user ID passed in for a bank account.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBankAccountWithZeroUserIdPassedIn()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 1;
                this.userId = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Save(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to be passed in for a bank account.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when bank account user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewBankAccountWithMismatchedUserIds()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Save(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to match the passed in user ID for a bank account.\r\nParameter name: account.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when merchant account has a user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewMerchantWithUserId()
            {
                // Arrange.
                this.account.IsUserAccount = false;
                this.account.UserId = 1;
                this.userId = null;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Save(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A merchant account is not expected to have a user ID.\r\nParameter name: account.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when user ID passed in for a merchant account.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewMerchantWithUserIdPassedIn()
            {
                // Arrange.
                this.account.IsUserAccount = false;
                this.account.UserId = null;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        AccountsRepository repository = new AccountsRepository(context);
                        repository.Save(this.account, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is not expected to be passed in for a merchant account.\r\nParameter name: userId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }
    }
}
