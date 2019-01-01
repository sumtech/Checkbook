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

        /// <summary>
        /// Tests for the Post() method.
        /// </summary>
        [TestClass]
        public class PostMethod : AccountsControllerTests
        {
            /// <summary>
            /// The account used as an input.
            /// </summary>
            private Account account;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// Stub of the account returned by the repository.
            /// </summary>
            private Account stubAccount;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                this.userId = 1;
                this.account = new Account();

                // Initialize the mock repository method.
                this.stubAccount = new Account();
                this.mockAccountsRepository
                    .Setup(m => m.Add(It.IsAny<Account>(), It.IsAny<long>()))
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
                IActionResult result = controller.Post(this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Add(this.account, this.userId), Times.Once, "The add method should have been called.");
                this.mockAccountsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubAccount, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsBadRequestErrorWhenAccountNull()
            {
                // Arrange.
                this.account = null;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Post(this.account);
                BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;

                // Assert.
                this.mockAccountsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(badRequestResult, "A bad request response should have been returned.");
                Assert.AreEqual(400, badRequestResult.StatusCode, "The status code from the response should have been 405.");
                string expectedMessage = "An account must be passed in for it to be saved.";
                Assert.AreEqual(expectedMessage, badRequestResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockAccountsRepository
                    .Setup(m => m.Add(It.IsAny<Account>(), It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Post(this.account);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error saving the account.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies the User ID gets added to the bank account before using the repository.
            /// </summary>
            [TestMethod]
            public void AddsUserIdToBankAccountWithZeroUserId()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 0;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Post(this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Add(It.Is<Account>(x => x.UserId == this.userId), It.IsAny<long>()), Times.Once, "The user ID should have been added to the entity.");
            }

            /// <summary>
            /// Verifies the User ID gets added to the bank account before using the repository.
            /// </summary>
            [TestMethod]
            public void AddsUserIdToBankAccountWithNullUserId()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = null;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Post(this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Add(It.Is<Account>(x => x.UserId == this.userId), It.IsAny<long>()), Times.Once, "The user ID should have been added to the entity.");
            }

            /// <summary>
            /// Verifies the User ID does not change for a bank account before using the repository.
            /// </summary>
            [TestMethod]
            public void IgnoresNonZeroUserIdForBankAccount()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 123;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Post(this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Add(It.Is<Account>(x => x.UserId == 123), It.IsAny<long>()), Times.Once, "The user ID for the entity should not have changed.");
            }

            /// <summary>
            /// Verifies the User ID does not get added to the merchant account before using the repository.
            /// </summary>
            [TestMethod]
            public void IgnoresZeroUserIdForMerchantAccount()
            {
                // Arrange.
                this.account.IsUserAccount = false;
                this.account.UserId = 0;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Post(this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Add(It.Is<Account>(x => x.UserId == 0), It.IsAny<long>()), Times.Once, "The user ID for the entity should not have changed.");
            }

            /// <summary>
            /// Verifies the User ID does not get added to the merchant account before using the repository.
            /// </summary>
            [TestMethod]
            public void IgnoresNonZeroUserIdForMerchantAccount()
            {
                // Arrange.
                this.account.IsUserAccount = false;
                this.account.UserId = 123;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Post(this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Add(It.Is<Account>(x => x.UserId == 123), It.IsAny<long>()), Times.Once, "The user ID for the entity should not have changed.");
            }
        }

        /// <summary>
        /// Tests for the Put() method.
        /// </summary>
        [TestClass]
        public class PutMethod : AccountsControllerTests
        {
            /// <summary>
            /// The id used as an input.
            /// </summary>
            private long id;

            /// <summary>
            /// The user ID used as an input.
            /// </summary>
            private long userId;

            /// <summary>
            /// The account used as an input.
            /// </summary>
            private Account account;

            /// <summary>
            /// Stub of the account returned by the repository.
            /// </summary>
            private Account stubAccount;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                this.id = 7;
                this.account = new Account
                {
                    Id = this.id,
                };
                this.userId = 1;

                // Initialize the mock repository method.
                this.stubAccount = new Account();
                this.mockAccountsRepository
                    .Setup(m => m.Save(It.IsAny<Account>(), It.IsAny<long>()))
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
                IActionResult result = controller.Put(this.id, this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Save(this.account, this.userId), Times.Once, "The save method should have been called.");
                this.mockAccountsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubAccount, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsBadRequestErrorWhenAccountNull()
            {
                // Arrange.
                this.account = null;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Put(this.id, this.account);
                BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;

                // Assert.
                this.mockAccountsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(badRequestResult, "A bad request response should have been returned.");
                Assert.AreEqual(400, badRequestResult.StatusCode, "The status code from the response should have been 405.");
                string expectedMessage = "An account must be passed in for it to be saved.";
                Assert.AreEqual(expectedMessage, badRequestResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsBadRequestErrorWhenAccountIdsMismatch()
            {
                // Arrange.
                this.id = this.id + 2;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Put(this.id, this.account);
                BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;

                // Assert.
                this.mockAccountsRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(badRequestResult, "A bad request response should have been returned.");
                Assert.AreEqual(400, badRequestResult.StatusCode, "The status code from the response should have been 405.");
                string expectedMessage = "The account ID values did not match.";
                Assert.AreEqual(expectedMessage, badRequestResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockAccountsRepository
                    .Setup(m => m.Save(It.IsAny<Account>(), It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Put(this.id, this.account);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error saving the account.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies the User ID gets added to the bank account before using the repository.
            /// </summary>
            [TestMethod]
            public void AddsUserIdToBankAccountWithZeroUserId()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 0;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Put(this.id, this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Save(It.Is<Account>(x => x.UserId == this.userId), It.IsAny<long>()), Times.Once, "The user ID should have been added to the entity.");
            }

            /// <summary>
            /// Verifies the User ID gets added to the bank account before using the repository.
            /// </summary>
            [TestMethod]
            public void AddsUserIdToBankAccountWithNullUserId()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = null;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Put(this.id, this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Save(It.Is<Account>(x => x.UserId == this.userId), It.IsAny<long>()), Times.Once, "The user ID should have been added to the entity.");
            }

            /// <summary>
            /// Verifies the User ID does not change for a bank account before using the repository.
            /// </summary>
            [TestMethod]
            public void IgnoresNonZeroUserIdForBankAccount()
            {
                // Arrange.
                this.account.IsUserAccount = true;
                this.account.UserId = 123;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Put(this.id, this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Save(It.Is<Account>(x => x.UserId == 123), It.IsAny<long>()), Times.Once, "The user ID for the entity should not have changed.");
            }

            /// <summary>
            /// Verifies the User ID does not get added to the merchant account before using the repository.
            /// </summary>
            [TestMethod]
            public void IgnoresZeroUserIdForMerchantAccount()
            {
                // Arrange.
                this.account.IsUserAccount = false;
                this.account.UserId = 0;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Put(this.id, this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Save(It.Is<Account>(x => x.UserId == 0), It.IsAny<long>()), Times.Once, "The user ID for the entity should not have changed.");
            }

            /// <summary>
            /// Verifies the User ID does not get added to the merchant account before using the repository.
            /// </summary>
            [TestMethod]
            public void IgnoresNonZeroUserIdForMerchantAccount()
            {
                // Arrange.
                this.account.IsUserAccount = false;
                this.account.UserId = 123;

                // Act.
                AccountsController controller = new AccountsController(this.mockAccountsRepository.Object);
                IActionResult result = controller.Put(this.id, this.account);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockAccountsRepository.Verify(m => m.Save(It.Is<Account>(x => x.UserId == 123), It.IsAny<long>()), Times.Once, "The user ID for the entity should not have changed.");
            }
        }
    }
}
