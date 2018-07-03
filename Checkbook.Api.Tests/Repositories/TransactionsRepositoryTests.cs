// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
            private IQueryable<Transaction> entities;

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
                }.AsQueryable();

                this.mockDbSet = new Mock<DbSet<Transaction>>();
                mockDbSet.As<IQueryable<Transaction>>().Setup(m => m.Provider).Returns(entities.Provider);
                mockDbSet.As<IQueryable<Transaction>>().Setup(m => m.Expression).Returns(entities.Expression);
                mockDbSet.As<IQueryable<Transaction>>().Setup(m => m.ElementType).Returns(entities.ElementType);
                mockDbSet.As<IQueryable<Transaction>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());

                // Initialize the repository response.
                this.mockDbContext
                    .Setup(m => m.Transactions)
                    .Returns(mockDbSet.Object);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void GetsTransactionsFromContext()
            {
                // Assert.
                TransactionsRepository repository = new TransactionsRepository(this.mockDbContext.Object);
                repository.GetTransactions();

                // Act.
                this.mockDbContext.Verify(m => m.Transactions, Times.Once);
                ////this.mockDbContext.VerifyNoOtherCalls();
                this.mockDbSet.Verify(m => m.AsQueryable(), Times.Once);
                ////this.mockDbSet.VerifyNoOtherCalls();
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void ReturnsTransactionsFromContext()
            {
                // Assert.
                TransactionsRepository repository = new TransactionsRepository(this.mockDbContext.Object);
                IEnumerable<Transaction> actual = repository.GetTransactions();

                Assert.AreEqual(2, actual.Count());

                var a = this.entities;
                var b = this.entities.AsEnumerable();
                var c = this.entities.AsEnumerable().First();

                var d = actual.First();

                Assert.AreEqual(this.entities.AsEnumerable().First(), actual.First());
                ////Assert.AreEqual(this.entities.ElementAt(0).Id, actual.ElementAt(0).Id);
                ////Assert.AreEqual(this.entities.ElementAt(1).Id, actual.ElementAt(1).Id);
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
