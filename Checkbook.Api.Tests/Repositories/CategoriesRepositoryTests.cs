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
    /// Tests for the <see cref="CategoriesRepository"/> class.
    /// </summary>
    [TestClass]
    public class CategoriesRepositoryTests
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
        public class GetAllMethod : CategoriesRepositoryTests
        {
            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Category> entities;

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
                    this.entities = ContextDataService.GetCategories(context)
                        .Where(a => a.UserId == this.userId)
                        .ToList();
                }
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsCategoriesFromContext()
            {
                // Act.
                List<Category> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    CategoriesRepository repository = new CategoriesRepository(context);
                    actualList = repository.GetAll(this.userId).ToList();
                }

                // Assert.
                Assert.AreEqual(this.entities.Count(), actualList.Count(), "The entity count should match.");

                Category expected = this.entities.ElementAt(0);
                Category actual = actualList.ElementAt(0);
                string index = "first";
                Assert.AreEqual(expected.Id, actual.Id, $"The ID for the {index} entity should match.");
                Assert.AreEqual(expected.Name, actual.Name, $"The name for the {index} entity should match.");
                Assert.AreEqual(expected.UserId, actual.UserId, $"The user ID for the {index} entity should match.");
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsCategoriesOnlyForTheUser()
            {
                // Act.
                List<Category> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    CategoriesRepository repository = new CategoriesRepository(context);
                    actualList = repository.GetAll(this.userId).ToList();
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    int numberOfCategoryUsers = context.Categories
                        .GroupBy(a => a.UserId)
                        .Count();
                    Assert.AreNotEqual(numberOfCategoryUsers, 0, "For the test to work there must be more than one user with categories.");
                    Assert.AreNotEqual(numberOfCategoryUsers, 1, "For the test to work there must be more than one user with categories.");
                }

                foreach (Category actual in actualList)
                {
                    Assert.AreEqual(this.userId, actual.UserId, "The user ID for each category should match the input user ID.");
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
                List<Category> actualList;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    CategoriesRepository repository = new CategoriesRepository(context);
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
        public class GetMethod : CategoriesRepositoryTests
        {
            /// <summary>
            /// The category ID used as an input.
            /// </summary>
            private long categoryId;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Category> entities;

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
                    this.entities = ContextDataService.GetCategories(context);
                }
            }

            /// <summary>
            /// Verifies the correct category from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsCategoryFromContext()
            {
                // Arrange.
                this.categoryId = 1;
                this.userId = 1;

                // Act.
                Category actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    CategoriesRepository repository = new CategoriesRepository(context);
                    actual = repository.Get(this.categoryId, this.userId);
                }

                // Assert.
                Category expected = this.entities.ElementAt(0);
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
                this.categoryId = 123;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        CategoriesRepository repository = new CategoriesRepository(context);
                        repository.Get(this.categoryId, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(NotFoundException), "This should be an argument exception.");
                string exceptionMessage = "The category was not found.";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when category user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenRecordFoundDoesNotBelongToTheUser()
            {
                // Arrange.
                this.categoryId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        CategoriesRepository repository = new CategoriesRepository(context);
                        repository.Get(this.categoryId, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(NotFoundException), "This should be an argument exception.");
                string exceptionMessage = "The category was not found.";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }

        /// <summary>
        /// Tests for the Add(category) method.
        /// </summary>
        [TestClass]
        public class AddMethod : CategoriesRepositoryTests
        {
            /// <summary>
            /// The category used as an input.
            /// </summary>
            private Category category;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Category> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the inputs.
                this.userId = 1;
                this.category = new Category
                {
                    Name = "Third Category",
                    UserId = this.userId,
                };

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetCategories(context);
                }
            }

            /// <summary>
            /// Verifies the correct entity from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsCategoryFromContext()
            {
                // Act.
                Category actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    CategoriesRepository repository = new CategoriesRepository(context);
                    actual = repository.Add(this.category, this.userId);
                }

                // Assert.
                long expectedId = 5;
                Assert.AreEqual(expectedId, actual.Id, "The ID for the entity should match.");
            }

            /// <summary>
            /// Verifies the new entity is in the context.
            /// </summary>
            [TestMethod]
            public void AddsCategoryToContext()
            {
                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    CategoriesRepository repository = new CategoriesRepository(context);
                    repository.Add(this.category, this.userId);
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    int expectedCategoryCount = 5;
                    Assert.AreEqual(expectedCategoryCount, context.Categories.Count(), "There should be the correct number of categories.");

                    long expectedId = 5;
                    Category actual = ContextDataService.GetCategoriesSet(context)
                        .FirstOrDefault(x => x.Id == expectedId);
                    Assert.IsNotNull(actual, "A category should be found.");

                    Category expected = this.category;
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
                this.category.Id = 5;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        CategoriesRepository repository = new CategoriesRepository(context);
                        repository.Add(this.category, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A new category without a specified ID should have been used. To update a category, use the Save method.\r\nParameter name: category.Id";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when buedget has a zero user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewCategoryWithZeroUserId()
            {
                // Arrange.
                this.category.UserId = 0;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        CategoriesRepository repository = new CategoriesRepository(context);
                        repository.Add(this.category, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A category is expected to have a user ID.\r\nParameter name: category.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when zero user ID passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenZeroUserIdPassedIn()
            {
                // Arrange.
                this.category.UserId = 1;
                this.userId = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        CategoriesRepository repository = new CategoriesRepository(context);
                        repository.Add(this.category, this.userId);
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
            /// Verifies exception when category user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenCategoryWithMismatchedUserIds()
            {
                // Arrange.
                this.category.UserId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        CategoriesRepository repository = new CategoriesRepository(context);
                        repository.Add(this.category, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to match the passed in user ID for a category.\r\nParameter name: category.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }

        /// <summary>
        /// Tests for the Save() method.
        /// </summary>
        [TestClass]
        public class SaveMethod : CategoriesRepositoryTests
        {
            /// <summary>
            /// The category used as an input.
            /// </summary>
            private Category category;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub data for the database set.
            /// </summary>
            private List<Category> entities;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the input.
                this.userId = 1;
                this.category = new Category
                {
                    Id = 2,
                    Name = "Second Category x",
                    UserId = this.userId,
                };

                // Initialize the database set.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    DatabaseSeed.AddEntities(context);
                    this.entities = ContextDataService.GetCategories(context);
                }
            }

            /// <summary>
            /// Verifies the correct entity from the context is returned.
            /// </summary>
            [TestMethod]
            public void ReturnsCategoryFromContext()
            {
                // Act.
                Category actual;
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    CategoriesRepository repository = new CategoriesRepository(context);
                    actual = repository.Save(this.category, this.userId);
                }

                // Assert.
                Assert.AreEqual(this.category.Id, actual.Id, "The ID for the entity should match.");
            }

            /// <summary>
            /// Verifies the entity was updated in the context.
            /// </summary>
            [TestMethod]
            public void UpdatesCategoryInTheContext()
            {
                // Act.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    CategoriesRepository repository = new CategoriesRepository(context);
                    repository.Save(this.category, this.userId);
                }

                // Assert.
                using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                {
                    int expectedCategoryCount = 4;
                    Assert.AreEqual(expectedCategoryCount, context.Categories.Count(), "The number of categories should not change.");

                    Category actual = ContextDataService.GetCategoriesSet(context)
                        .FirstOrDefault(x => x.Id == this.category.Id);
                    Assert.IsNotNull(actual, "A category should be found.");

                    Category expected = this.category;
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
                this.category.Id = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        CategoriesRepository repository = new CategoriesRepository(context);
                        repository.Save(this.category, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A category with a specified ID should have been used. To add a category, use the Add method.\r\nParameter name: category.Id";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when category has a zero user ID.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenCategoryWithZeroUserId()
            {
                // Arrange.
                this.category.UserId = 0;
                this.userId = 1;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        CategoriesRepository repository = new CategoriesRepository(context);
                        repository.Save(this.category, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A category is expected to have a user ID.\r\nParameter name: category.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }

            /// <summary>
            /// Verifies exception when null user ID passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenNewZeroUserIdPasedIn()
            {
                // Arrange.
                this.category.UserId = 1;
                this.userId = 0;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        CategoriesRepository repository = new CategoriesRepository(context);
                        repository.Save(this.category, this.userId);
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
            /// Verifies exception when category user ID does not match the one passed in.
            /// </summary>
            [TestMethod]
            public void ThrowsExceptionWhenCategoryWithMismatchedUserIds()
            {
                // Arrange.
                this.category.UserId = 1;
                this.userId = 2;

                // Act.
                Exception caughtException = null;
                try
                {
                    using (CheckbookContext context = new CheckbookContext(this.dbContextOptions))
                    {
                        CategoriesRepository repository = new CategoriesRepository(context);
                        repository.Save(this.category, this.userId);
                    }
                }
                catch (Exception ex)
                {
                    caughtException = ex;
                }

                // Assert.
                Assert.IsNotNull(caughtException, "An exception should be thrown.");
                Assert.IsInstanceOfType(caughtException, typeof(ArgumentException), "This should be an argument exception.");
                string exceptionMessage = "A user ID is expected to match the passed in user ID for a category.\r\nParameter name: category.UserId";
                Assert.AreEqual(exceptionMessage, caughtException.Message, "The exception message should be correct.");
            }
        }
    }
}
