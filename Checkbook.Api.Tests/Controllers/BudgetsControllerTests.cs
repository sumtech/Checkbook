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
    /// Tests for the <see cref="BudgetsController"/> class.
    /// </summary>
    public class BudgetsControllerTests
    {
        /// <summary>
        /// The mock implementation of the budgets repository.
        /// </summary>
        private Mock<IBudgetsRepository> mockBudgetsRepository;

        /// <summary>
        /// Initializes the tests for the class.
        /// </summary>
        [TestInitialize]
        public virtual void Initialize()
        {
            // Initialize the mock object(s).
            this.mockBudgetsRepository = new Mock<IBudgetsRepository>();
        }

        /// <summary>
        /// Tests for the parameterless Get() method.
        /// </summary>
        [TestClass]
        public class GetMethod : BudgetsControllerTests
        {
            /// <summary>
            /// The user ID.
            /// </summary>
            private long userId;

            /// <summary>
            /// The stub repository response.
            /// </summary>
            private List<Budget> stubBudgets;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                this.userId = 1;

                // Initialize the mock repository method.
                this.stubBudgets = new List<Budget>();
                this.mockBudgetsRepository
                    .Setup(m => m.GetAll(It.IsAny<long>()))
                    .Returns(this.stubBudgets);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                BudgetsController controller = new BudgetsController(this.mockBudgetsRepository.Object);
                IActionResult result = controller.Get();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockBudgetsRepository.Verify(m => m.GetAll(this.userId), Times.Once, "The budgets should have been requested from the repository.");
                this.mockBudgetsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubBudgets, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies an empty list is returned when the repository returns null.
            /// </summary>
            [TestMethod]
            public void ReturnsAnEmptyListWhenRepositoryReturnsNull()
            {
                // Arrange.
                this.stubBudgets = null;
                this.mockBudgetsRepository
                    .Setup(m => m.GetAll(It.IsAny<long>()))
                    .Returns(this.stubBudgets);

                // Act.
                BudgetsController controller = new BudgetsController(this.mockBudgetsRepository.Object);
                IActionResult result = controller.Get();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockBudgetsRepository.Verify(m => m.GetAll(this.userId), Times.Once, "The budgets should have been requested from the repository.");
                this.mockBudgetsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(0, (okResult.Value as List<Budget>).Count, "An empty list should be the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockBudgetsRepository
                    .Setup(m => m.GetAll(It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                BudgetsController controller = new BudgetsController(this.mockBudgetsRepository.Object);
                IActionResult result = controller.Get();
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error getting the budgets.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }
        }

        /// <summary>
        /// Tests for the Get() method that takes in an ID value.
        /// </summary>
        [TestClass]
        public class GetMethod_Id : BudgetsControllerTests
        {
            /// <summary>
            /// The user ID.
            /// </summary>
            private long userId;

            /// <summary>
            /// The ID used as an input.
            /// </summary>
            private long id;

            /// <summary>
            /// The stub repository response.
            /// </summary>
            private Budget stubBudget;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the input(s).
                this.userId = 1;
                this.id = 7;

                // Initialize the mock repository method.
                this.stubBudget = new Budget();
                this.mockBudgetsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns(this.stubBudget);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                BudgetsController controller = new BudgetsController(this.mockBudgetsRepository.Object);
                IActionResult result = controller.Get(id);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                mockBudgetsRepository.Verify(m => m.Get(this.id, this.userId), Times.Once, "The budgets should have been requested from the repository.");
                mockBudgetsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubBudget, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies an empty list is returned when the repository returns null.
            /// </summary>
            [TestMethod]
            public void ReturnsNotFoundWhenRepositoryReturnsNull()
            {
                // Arrange.
                this.stubBudget = null;
                this.mockBudgetsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns(this.stubBudget);

                // Act.
                BudgetsController controller = new BudgetsController(this.mockBudgetsRepository.Object);
                IActionResult result = controller.Get(id);
                NotFoundResult notFoundResult = result as NotFoundResult;

                // Assert.
                mockBudgetsRepository.Verify(m => m.Get(this.id, this.userId), Times.Once, "The budget should have been requested from the repository.");
                mockBudgetsRepository.VerifyNoOtherCalls();

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
                this.mockBudgetsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns<Budget>(null);

                // Act.
                BudgetsController controller = new BudgetsController(this.mockBudgetsRepository.Object);
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
                this.mockBudgetsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                BudgetsController controller = new BudgetsController(this.mockBudgetsRepository.Object);
                IActionResult result = controller.Get(id);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error getting the budget.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }
        }
    }
}
