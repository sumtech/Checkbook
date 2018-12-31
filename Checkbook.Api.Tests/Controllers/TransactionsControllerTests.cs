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
        /// The user ID.
        /// </summary>
        private long userId = 1;

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
                    .Setup(m => m.GetAll(It.IsAny<long>()))
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
                this.mockTransactionsRepository.Verify(m => m.GetAll(1), Times.Once, "The transactions should have been requested from the repository.");
                this.mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubTransactions, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies an empty list is returned when the repository returns null.
            /// </summary>
            [TestMethod]
            public void ReturnsAnEmptyListWhenRepositoryReturnsNull()
            {
                // Arrange.
                this.stubTransactions = null;
                this.mockTransactionsRepository
                    .Setup(m => m.GetAll(this.userId))
                    .Returns(this.stubTransactions);

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockTransactionsRepository.Verify(m => m.GetAll(this.userId), Times.Once, "The transactions should have been requested from the repository.");
                this.mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(0, (okResult.Value as List<Transaction>).Count, "An empty list should be the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.GetAll(It.IsAny<long>()))
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
            /// The transaction ID used as an input.
            /// </summary>
            private long transactionId;

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
                this.transactionId = 7;

                // Initialize the mock repository method.
                this.stubTransaction = new Transaction();
                this.mockTransactionsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
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
                IActionResult result = controller.Get(transactionId);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                mockTransactionsRepository.Verify(m => m.Get(this.transactionId, this.userId), Times.Once, "The transactions should have been requested from the repository.");
                mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubTransaction, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsNotFoundWhenRepositoryReturnsNull()
            {
                // Arrange.
                this.stubTransaction = null;
                this.mockTransactionsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns(this.stubTransaction);

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(transactionId);
                NotFoundResult notFoundResult = result as NotFoundResult;

                // Assert.
                mockTransactionsRepository.Verify(m => m.Get(this.transactionId, this.userId), Times.Once, "The transaction should have been requested from the repository.");
                mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(notFoundResult, "A Not Found response should have been returned.");
                Assert.AreEqual(404, notFoundResult.StatusCode, "The status code from the response should have been 404.");
            }

            /// <summary>
            /// Verifies the correct result is returned when the entity is not found.
            /// </summary>
            [TestMethod]
            public void HandlesNotFound()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns<Transaction>(null);

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(transactionId);
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
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(transactionId);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error getting the transaction.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }
        }

        /// <summary>
        /// Tests for the Post() method.
        /// </summary>
        [TestClass]
        public class PostMethod : TransactionsControllerTests
        {
            /// <summary>
            /// The transaction used as an input.
            /// </summary>
            private Transaction transaction;

            /// <summary>
            /// Stub of the transaction returned by the repository.
            /// </summary>
            private Transaction stubTransaction;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                this.transaction = new Transaction();

                // Initialize the mock repository method.
                this.stubTransaction = new Transaction();
                this.mockTransactionsRepository
                    .Setup(m => m.Add(It.IsAny<Transaction>(), It.IsAny<long>()))
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
                IActionResult result = controller.Post(this.transaction);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockTransactionsRepository.Verify(m => m.Add(this.transaction, this.userId), Times.Once, "The add method should have been called.");
                this.mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubTransaction, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsBadRequestErrorWhenTransactionNull()
            {
                // Arrange.
                this.transaction = null;

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Post(this.transaction);
                BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;

                // Assert.
                this.mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(badRequestResult, "A bad request response should have been returned.");
                Assert.AreEqual(400, badRequestResult.StatusCode, "The status code from the response should have been 405.");
                string expectedMessage = "A transaction must be passed in for it to be saved.";
                Assert.AreEqual(expectedMessage, badRequestResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.Add(It.IsAny<Transaction>(), It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Post(this.transaction);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error saving the transaction.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies the User ID gets added to the entity before using the repository.
            /// </summary>
            [TestMethod]
            public void AddsUserIdToEntity()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Post(this.transaction);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockTransactionsRepository.Verify(m => m.Add(It.Is<Transaction>(x => x.UserId == this.userId), It.IsAny<long>()), Times.Once, "The user ID should have been added to the entity.");
            }
        }

        /// <summary>
        /// Tests for the Put() method.
        /// </summary>
        [TestClass]
        public class PutMethod : TransactionsControllerTests
        {
            /// <summary>
            /// The id used as an input.
            /// </summary>
            private long id;

            /// <summary>
            /// The transaction used as an input.
            /// </summary>
            private Transaction transaction;

            /// <summary>
            /// Stub of the transaction returned by the repository.
            /// </summary>
            private Transaction stubTransaction;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                this.id = 7;
                this.transaction = new Transaction
                {
                    Id = this.id,
                };

                // Initialize the mock repository method.
                this.stubTransaction = new Transaction();
                this.mockTransactionsRepository
                    .Setup(m => m.Save(It.IsAny<Transaction>(), It.IsAny<long>()))
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
                IActionResult result = controller.Put(this.id, this.transaction);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockTransactionsRepository.Verify(m => m.Save(this.transaction, this.userId), Times.Once, "The save method should have been called.");
                this.mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubTransaction, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsBadRequestErrorWhenTransactionNull()
            {
                // Arrange.
                this.transaction = null;

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Put(this.id, this.transaction);
                BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;

                // Assert.
                this.mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(badRequestResult, "A bad request response should have been returned.");
                Assert.AreEqual(400, badRequestResult.StatusCode, "The status code from the response should have been 405.");
                string expectedMessage = "A transaction must be passed in for it to be saved.";
                Assert.AreEqual(expectedMessage, badRequestResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsBadRequestErrorWhenTransactionIdsMismatch()
            {
                // Arrange.
                this.id = this.id + 2;

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Put(this.id, this.transaction);
                BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;

                // Assert.
                this.mockTransactionsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(badRequestResult, "A bad request response should have been returned.");
                Assert.AreEqual(400, badRequestResult.StatusCode, "The status code from the response should have been 405.");
                string expectedMessage = "The transaction ID values did not match.";
                Assert.AreEqual(expectedMessage, badRequestResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.Save(It.IsAny<Transaction>(), It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Put(this.id, this.transaction);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error saving the transaction.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies the User ID gets added to the entity before using the repository.
            /// </summary>
            [TestMethod]
            public void AddsUserIdToEntity()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Put(this.id, this.transaction);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockTransactionsRepository.Verify(m => m.Save(It.Is<Transaction>(x => x.UserId == this.userId), It.IsAny<long>()), Times.Once, "The user ID should have been added to the entity.");
            }
        }
    }
}
