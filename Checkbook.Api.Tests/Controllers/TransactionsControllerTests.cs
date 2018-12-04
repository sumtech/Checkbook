// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

namespace Checkbook.Api.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Checkbook.Api.Controllers;
    using Checkbook.Api.Models;
    using Checkbook.Api.Repositories;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests for the <see cref="TransactionsController"/> class.
    /// </summary>
    public class TransactionsControllerTests
    {
        /// <summary>
        /// The mock implementation of the transactions repository.
        /// </summary>
        private Mock<ITransactionsRepository> mockTransactionsRepository;

        /// <summary>
        /// Initializes the tests for the class.
        /// </summary>
        [TestInitialize]
        public virtual void Initialize()
        {
            // Initialize the mock object(s).
            this.mockTransactionsRepository = new Mock<ITransactionsRepository>();
        }

        /// <summary>
        /// Tests for the parameterless Get() method.
        /// </summary>
        [TestClass]
        public class GetMethod : TransactionsControllerTests
        {
            /// <summary>
            /// The stub repository response.
            /// </summary>
            private List<Transaction> stubTransactions;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the mock repository method.
                this.stubTransactions = new List<Transaction>();
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransactions())
                    .Returns(this.stubTransactions);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockTransactionsRepository.Verify(m => m.GetTransactions(), Times.Once, "The transactions should have been requested from the repository.");
                this.mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubTransactions, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransactions())
                    .Throws(new Exception());

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get();
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error getting the transactions.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }
        }

        /// <summary>
        /// Tests for the Get() method that takes in an ID value.
        /// </summary>
        [TestClass]
        public class GetMethod_Id : TransactionsControllerTests
        {
            /// <summary>
            /// The ID used as an input.
            /// </summary>
            private long id;

            /// <summary>
            /// The stub repository response.
            /// </summary>
            private Transaction stubTransaction;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the input(s).
                this.id = 7;

                // Initialize the mock repository method.
                this.stubTransaction = new Transaction();
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransaction(It.IsAny<long>()))
                    .Returns(this.stubTransaction);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(id);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                mockTransactionsRepository.Verify(m => m.GetTransaction(this.id), Times.Once, "The transactions should have been requested from the repository.");
                mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubTransaction, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies the correct result is returned when the entity is not found.
            /// </summary>
            [TestMethod]
            public void HandlesNotFound()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransaction(It.IsAny<long>()))
                    .Returns<Transaction>(null);

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(id);
                NotFoundResult notFoundResult = result as NotFoundResult;

                // Assert.
                Assert.IsNotNull(notFoundResult, "An object result should have been returned.");
                Assert.AreEqual(404, notFoundResult.StatusCode, "The status code from the response should have been 404.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransaction(It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(id);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error getting the transaction.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }
        }
    }
}
