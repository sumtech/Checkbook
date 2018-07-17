// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests for the <see cref="TransactionsRepository"/> class.
    /// </summary>
    [TestClass]
    public class TransactionsRepositoryTests
    {
        /// <summary>
        /// The mock implementation of the database context.
        /// </summary>
        private Mock<CheckbookContext> mockDbContext;

        /// <summary>
        /// Initializes the tests for the class.
        /// </summary>
        [TestInitialize]
        public virtual void Initialize()
        {
            // Initialize the mocks.
            DbContextOptions<CheckbookContext> options = new DbContextOptions<CheckbookContext>();
            this.mockDbContext = new Mock<CheckbookContext>(options);
        }

        /// <summary>
        /// Tests for the GetTransactions() method.
        /// </summary>
        [TestClass]
        public class GetTransactionsMethod : TransactionsRepositoryTests
        {
            /// <summary>
            /// Mock data for the database set.
            /// </summary>
            private List<Transaction> entities;

            /// <summary>
            /// Mock implementation of the database set.
            /// </summary>
            private Mock<DbSet<Transaction>> mockDbSet;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the database set.
                this.entities = new List<Transaction>
                {
                    new Transaction
                    {
                        Id = 1,
                    },
                    new Transaction
                    {
                        Id = 2,
                    },
                };

                IQueryable<Transaction> queryableEntities = this.entities.AsQueryable();

                this.mockDbSet = new Mock<DbSet<Transaction>>();
                this.mockDbSet.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(queryableEntities.Provider);
                this.mockDbSet.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(queryableEntities.Expression);
                this.mockDbSet.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(queryableEntities.ElementType);
                this.mockDbSet.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(queryableEntities.GetEnumerator());

                // Initialize the repository response.
                this.mockDbContext
                    .Setup(m => m.Transactions)
                    .Returns(mockDbSet.Object);
            }

            /// <summary>
            /// Verifies the entiies from the context are returned.
            /// </summary>
            [TestMethod]
            public void ReturnsTransactionsFromContext()
            {
                // Assert.
                TransactionsRepository repository = new TransactionsRepository(this.mockDbContext.Object);
                IEnumerable<Transaction> actual = repository.GetTransactions();

                // Act.
                this.mockDbContext.Verify(m => m.Transactions, Times.Once);
                ////this.mockDbSet.Verify(m => m.Include(t => t.Merchant), Times.Once, "The merchant should have been included.");
                ////this.mockDbSet.Verify(m => m.Include(t => t.BankAccount), Times.Once, "The bank account should have been included.");
                ////this.mockDbSet.Verify(m => m.AsEnumerable(), Times.Once);
                ////this.mockDbContext.VerifyNoOtherCalls();
                ////this.mockDbSet.VerifyNoOtherCalls();

                Assert.AreEqual(this.entities.Count(), actual.Count(), "The entity entity count should have matched.");

                List<Transaction> actualList = actual.ToList();
                Assert.AreEqual(this.entities[0], actualList[0], "The first entities should have been the same.");
                Assert.AreEqual(this.entities[1], actualList[1], "The second entities should have been the same.");
                ////Assert.AreEqual(this.entities.ElementAt(0).Id, actual.ElementAt(0).Id);
                ////Assert.AreEqual(this.entities.ElementAt(1).Id, actual.ElementAt(1).Id);
            }

            /////// <summary>
            /////// Verifies.
            /////// </summary>
            ////[TestMethod]
            ////public void ReturnsTransactionsFromContext()
            ////{
            ////    // Assert.
            ////    TransactionsRepository repository = new TransactionsRepository(this.mockDbContext.Object);
            ////    IEnumerable<Transaction> actual = repository.GetTransactions();

            ////    Assert.AreEqual(2, actual.Count());

            ////    var a = this.entities;
            ////    var b = this.entities.AsEnumerable();
            ////    var c = this.entities.AsEnumerable().First();

            ////    var d = actual.First();

            ////    Assert.AreEqual(this.entities.AsEnumerable().First(), actual.First());
            ////    ////Assert.AreEqual(this.entities.ElementAt(0).Id, actual.ElementAt(0).Id);
            ////    ////Assert.AreEqual(this.entities.ElementAt(1).Id, actual.ElementAt(1).Id);
            ////}

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void AllowsExceptionWhenContextThrowsException()
            {
                Assert.Inconclusive();
            }
        }

        /// <summary>
        /// Tests for the GetTransaction() method.
        /// </summary>
        [TestClass]
        public class GetTransactionMethod : TransactionsRepositoryTests
        {
            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void GetsTransactionFromContext()
            {
                Assert.Inconclusive();
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void ReturnsTransactionFromContext()
            {
                Assert.Inconclusive();
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void AllowsExceptionWhenContextThrowsException()
            {
                Assert.Inconclusive();
            }
        }

        /// <summary>
        /// Tests for the AddTransaction() method.
        /// </summary>
        [TestClass]
        public class AddTransactionMethod : TransactionsRepositoryTests
        {
            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void AddsTransactionToContext()
            {
                Assert.Inconclusive();
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void SavesChanges()
            {
                Assert.Inconclusive();
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void ReturnsSavedTransaction()
            {
                Assert.Inconclusive();
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void AllowsExceptionWhenContextThrowsException()
            {
                Assert.Inconclusive();
            }
        }
    }
}
