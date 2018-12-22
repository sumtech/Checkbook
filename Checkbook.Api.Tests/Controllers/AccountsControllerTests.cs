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
    /// Tests for the <see cref="AccountsController"/> class.
    /// </summary>
    public class AccountsControllerTests
    {
        /// <summary>
        /// The mock implementation of the accounts repository.
        /// </summary>
        private Mock<IAccountsRepository> mockAccountsRepository;

        /// <summary>
        /// Initializes the tests for the class.
        /// </summary>
        [TestInitialize]
        public virtual void Initialize()
        {
            // Initialize the mock object(s).
            this.mockAccountsRepository = new Mock<IAccountsRepository>();
        }

        /// <summary>
        /// Tests for the parameterless GetBankAccounts() method.
        /// </summary>
        [TestClass]
        public class GetBankAccountsMethod : AccountsControllerTests
        {
            /// <summary>
            /// The user ID.
            /// </summary>
            private long userId;

            /// <summary>
            /// The stub repository response.
            /// </summary>
            private List<Account> stubAccounts;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                this.userId = 1;

                // Initialize the mock repository method.
                this.stubAccounts = new List<Account>();
                this.mockAccountsRepository
                    .Setup(m => m.GetBankAccounts(It.IsAny<long>()))
                    .Returns(this.stubAccounts);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.GetBankAccounts();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.GetBankAccounts(this.userId), Times.Once, "The accounts should have been requested from the repository.");
                this.mockAccountsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubAccounts, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies an empty list is returned when the repository returns null.
            /// </summary>
            [TestMethod]
            public void ReturnsAnEmptyListWhenRepositoryReturnsNull()
            {
                // Arrange.
                this.stubAccounts = null;
                this.mockAccountsRepository
                    .Setup(m => m.GetBankAccounts(It.IsAny<long>()))
                    .Returns(this.stubAccounts);

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.GetBankAccounts();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.GetBankAccounts(this.userId), Times.Once, "The accounts should have been requested from the repository.");
                this.mockAccountsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(0, (okResult.Value as List<Account>).Count, "An empty list should be the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockAccountsRepository
                    .Setup(m => m.GetBankAccounts(It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.GetBankAccounts();
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error getting the bank accounts.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }
        }

        /// <summary>
        /// Tests for the parameterless GetMerchantAccounts() method.
        /// </summary>
        [TestClass]
        public class GetMerchantAccountsMethod : AccountsControllerTests
        {
            /// <summary>
            /// The stub repository response.
            /// </summary>
            private List<Account> stubAccounts;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                // Initialize the mock repository method.
                this.stubAccounts = new List<Account>();
                this.mockAccountsRepository
                    .Setup(m => m.GetMerchantAccounts())
                    .Returns(this.stubAccounts);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.GetMerchantAccounts();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.GetMerchantAccounts(), Times.Once, "The accounts should have been requested from the repository.");
                this.mockAccountsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubAccounts, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies an empty list is returned when the repository returns null.
            /// </summary>
            [TestMethod]
            public void ReturnsAnEmptyListWhenRepositoryReturnsNull()
            {
                // Arrange.
                this.stubAccounts = null;
                this.mockAccountsRepository
                    .Setup(m => m.GetMerchantAccounts())
                    .Returns(this.stubAccounts);

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.GetMerchantAccounts();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.GetMerchantAccounts(), Times.Once, "The accounts should have been requested from the repository.");
                this.mockAccountsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(0, (okResult.Value as List<Account>).Count, "An empty list should be the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockAccountsRepository
                    .Setup(m => m.GetMerchantAccounts())
                    .Throws(new Exception());

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.GetMerchantAccounts();
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error getting the merchant accounts.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }
        }

        /// <summary>
        /// Tests for the Get() method that takes in an ID value.
        /// </summary>
        [TestClass]
        public class GetMethod_Id : AccountsControllerTests
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
            private Account stubAccount;

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
                this.stubAccount = new Account();
                this.mockAccountsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns(this.stubAccount);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Get(id);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                mockAccountsRepository.Verify(m => m.Get(this.id, this.userId), Times.Once, "The accounts should have been requested from the repository.");
                mockAccountsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubAccount, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies an empty list is returned when the repository returns null.
            /// </summary>
            [TestMethod]
            public void ReturnsNotFoundWhenRepositoryReturnsNull()
            {
                // Arrange.
                this.stubAccount = null;
                this.mockAccountsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns(this.stubAccount);

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Get(id);
                NotFoundResult notFoundResult = result as NotFoundResult;

                // Assert.
                mockAccountsRepository.Verify(m => m.Get(this.id, this.userId), Times.Once, "The account should have been requested from the repository.");
                mockAccountsRepository.VerifyNoOtherCalls();

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
                this.mockAccountsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns<Account>(null);

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
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
                this.mockAccountsRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Get(id);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error getting the account.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }
        }
    }
}
