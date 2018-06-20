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
            /// The stub exception thrown by the repository.
            /// </summary>
            private Exception stubException;

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

                this.stubException = new Exception();
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void CallsRepositoryMethod()
            {
                // Arrange.
                Mock<ITransactionsRepository> mockTransactionsRepository = new Mock<ITransactionsRepository>();
                mockTransactionsRepository
                    .Setup(x => x.GetTransactions())
                    .Returns(this.stubTransactions);

                // Act.
                TransactionsController controller = new TransactionsController(mockTransactionsRepository.Object);
                controller.Get();

                // Assert.
                mockTransactionsRepository.Verify(m => m.GetTransactions(), Times.Once);
                mockTransactionsRepository.VerifyNoOtherCalls();
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void ReturnsOkResult()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                Assert.IsNotNull(okResult);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void Returns200Status()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                Assert.IsNotNull(okResult);
                Assert.AreEqual(200, okResult.StatusCode);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void RetursResponseFromRepository()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                Assert.IsNotNull(okResult);
                Assert.AreEqual(this.stubTransactions, okResult.Value);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void ReturnsObjectResultWhenRepositoryThrowsException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransactions())
                    .Throws(this.stubException);

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get();
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsInstanceOfType(result, typeof(ObjectResult));
                Assert.IsNotNull(objectResult);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void Returns500StatusWhenRepositoryThrowsException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransactions())
                    .Throws(this.stubException);

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get();
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult);
                Assert.AreEqual(500, objectResult.StatusCode);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void ReturnsMessageWhenRepositoryThrowsException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransactions())
                    .Throws(this.stubException);

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get();
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                string expected = "There was an error getting the transactions.";
                Assert.IsNotNull(objectResult);
                Assert.AreEqual(expected, objectResult.Value);
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
            private Guid id;

            /// <summary>
            /// The stub repository response.
            /// </summary>
            private Transaction stubTransaction;

            /// <summary>
            /// The stub exception thrown by the repository.
            /// </summary>
            private Exception stubException;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the input(s).
                this.id = Guid.NewGuid();

                // Initialize the mock repository method.
                this.stubTransaction = new Transaction();
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransaction(It.IsAny<Guid>()))
                    .Returns(this.stubTransaction);

                this.stubException = new Exception();
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void CallsRepositoryMethod()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                controller.Get(id);

                // Assert.
                this.mockTransactionsRepository.Verify(m => m.GetTransaction(this.id), Times.Once);
                this.mockTransactionsRepository.VerifyNoOtherCalls();
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void ReturnsOkResult()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(id);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                Assert.IsInstanceOfType(result, typeof(OkObjectResult));
                Assert.IsNotNull(okResult);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void Returns200Status()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(id);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                Assert.IsNotNull(okResult);
                Assert.AreEqual(200, okResult.StatusCode);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void RetursResponseFromRepository()
            {
                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(id);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                Assert.IsNotNull(okResult);
                Assert.AreEqual(this.stubTransaction, okResult.Value);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void ReturnsObjectResultWhenRepositoryThrowsException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransaction(It.IsAny<Guid>()))
                    .Throws(this.stubException);

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(id);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsInstanceOfType(result, typeof(ObjectResult));
                Assert.IsNotNull(objectResult);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void Returns500StatusWhenRepositoryThrowsException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransaction(It.IsAny<Guid>()))
                    .Throws(this.stubException);

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(id);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult);
                Assert.AreEqual(500, objectResult.StatusCode);
            }

            /// <summary>
            /// Verifies.
            /// </summary>
            [TestMethod]
            public void ReturnsMessageWhenRepositoryThrowsException()
            {
                // Arrange.
                this.mockTransactionsRepository
                    .Setup(m => m.GetTransaction(It.IsAny<Guid>()))
                    .Throws(this.stubException);

                // Act.
                TransactionsController controller = new TransactionsController(this.mockTransactionsRepository.Object);
                IActionResult result = controller.Get(id);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                string expected = "There was an error getting the transaction.";
                Assert.IsNotNull(objectResult);
                Assert.AreEqual(expected, objectResult.Value);
            }
        }
    }
}
