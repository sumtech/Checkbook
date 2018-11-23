////// Copyright (c) Palouse Coding Conglomerate. All Rights Reserved.

////namespace Checkbook.Api.Tests.Controllers
////{
////    using System;
////    using System.Collections.Generic;
////    using Checkbook.Api.Controllers;
////    using Checkbook.Api.Models;
////    using Checkbook.Api.Repositories;
////    using Microsoft.AspNetCore.Mvc;
////    using Microsoft.VisualStudio.TestTools.UnitTesting;
////    using Moq;

////    /// <summary>
////    /// Tests for the <see cref="MerchantsController"/> class.
////    /// </summary>
////    public class MerchantsControllerTests
////    {
////        /// <summary>
////        /// The mock implementation of the merchants repository.
////        /// </summary>
////        private Mock<IMerchantsRepository> mockMerchantsRepository;

////        /// <summary>
////        /// Initializes the tests for the class.
////        /// </summary>
////        [TestInitialize]
////        public virtual void Initialize()
////        {
////            // Initialize the mock object(s).
////            this.mockMerchantsRepository = new Mock<IMerchantsRepository>();
////        }

////        /// <summary>
////        /// Tests for the parameterless Get() method.
////        /// </summary>
////        [TestClass]
////        public class GetMethod : MerchantsControllerTests
////        {
////            /// <summary>
////            /// The stub repository response.
////            /// </summary>
////            private List<Merchant> stubMerchants;

////            /// <summary>
////            /// Initializes the tests for the method.
////            /// </summary>
////            [TestInitialize]
////            public override void Initialize()
////            {
////                base.Initialize();

////                // Initialize the mock repository method.
////                this.stubMerchants = new List<Merchant>();
////                this.mockMerchantsRepository
////                    .Setup(m => m.GetMerchants())
////                    .Returns(this.stubMerchants);
////            }

////            /// <summary>
////            /// Verifies the result from the repository is retrieved correctly.
////            /// </summary>
////            [TestMethod]
////            public void ReturnsRepositoryResult()
////            {
////                // Arrange.
////                Mock<IMerchantsRepository> mockMerchantsRepository = new Mock<IMerchantsRepository>();
////                mockMerchantsRepository
////                    .Setup(x => x.GetMerchants())
////                    .Returns(this.stubMerchants);

////                // Act.
////                MerchantsController controller = new MerchantsController(mockMerchantsRepository.Object);
////                IActionResult result = controller.Get();
////                OkObjectResult okResult = result as OkObjectResult;

////                // Assert.
////                mockMerchantsRepository.Verify(m => m.GetMerchants(), Times.Once, "The merchants should have been requested from the repository.");
////                mockMerchantsRepository.VerifyNoOtherCalls();

////                Assert.IsNotNull(okResult, "An OK response should have been returned.");
////                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
////                Assert.AreEqual(this.stubMerchants, okResult.Value, "The result from the repository should have been returned.");
////            }

////            /// <summary>
////            /// Verifies that general exceptions are handled correctly.
////            /// </summary>
////            [TestMethod]
////            public void HandlesGeneralException()
////            {
////                // Arrange.
////                this.mockMerchantsRepository
////                    .Setup(m => m.GetMerchants())
////                    .Throws(new Exception());

////                // Act.
////                MerchantsController controller = new MerchantsController(this.mockMerchantsRepository.Object);
////                IActionResult result = controller.Get();
////                ObjectResult objectResult = result as ObjectResult;

////                // Assert.
////                Assert.IsNotNull(objectResult, "An object result should have been returned.");
////                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
////                string expectedMessage = "There was an error getting the merchants.";
////                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
////            }
////        }

////        /// <summary>
////        /// Tests for the Get() method that takes in an ID value.
////        /// </summary>
////        [TestClass]
////        public class GetMethod_Id : MerchantsControllerTests
////        {
////            /// <summary>
////            /// The ID used as an input.
////            /// </summary>
////            private long id;

////            /// <summary>
////            /// The stub repository response.
////            /// </summary>
////            private Merchant stubMerchant;

////            /// <summary>
////            /// Initializes the tests for the method.
////            /// </summary>
////            [TestInitialize]
////            public override void Initialize()
////            {
////                base.Initialize();

////                // Initialize the input(s).
////                this.id = 7;

////                // Initialize the mock repository method.
////                this.stubMerchant = new Merchant();
////                this.mockMerchantsRepository
////                    .Setup(m => m.GetMerchant(It.IsAny<long>()))
////                    .Returns(this.stubMerchant);
////            }

////            /// <summary>
////            /// Verifies the result from the repository is retrieved correctly.
////            /// </summary>
////            [TestMethod]
////            public void ReturnsRepositoryResult()
////            {
////                // Act.
////                MerchantsController controller = new MerchantsController(this.mockMerchantsRepository.Object);
////                IActionResult result = controller.Get(id);
////                OkObjectResult okResult = result as OkObjectResult;

////                // Assert.
////                mockMerchantsRepository.Verify(m => m.GetMerchant(this.id), Times.Once, "The merchants should have been requested from the repository.");
////                mockMerchantsRepository.VerifyNoOtherCalls();

////                Assert.IsNotNull(okResult, "An OK response should have been returned.");
////                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
////                Assert.AreEqual(this.stubMerchant, okResult.Value, "The result from the repository should have been returned.");
////            }

////            /// <summary>
////            /// Verifies the correct result is returned when the entity is not found.
////            /// </summary>
////            [TestMethod]
////            public void HandlesNotFound()
////            {
////                // Arrange.
////                this.mockMerchantsRepository
////                    .Setup(m => m.GetMerchant(It.IsAny<long>()))
////                    .Returns<Merchant>(null);

////                // Act.
////                MerchantsController controller = new MerchantsController(this.mockMerchantsRepository.Object);
////                IActionResult result = controller.Get(id);
////                NotFoundResult notFoundResult = result as NotFoundResult;

////                // Assert.
////                Assert.IsNotNull(notFoundResult, "An object result should have been returned.");
////                Assert.AreEqual(404, notFoundResult.StatusCode, "The status code from the response should have been 404.");
////            }

////            /// <summary>
////            /// Verifies that general exceptions are handled correctly.
////            /// </summary>
////            [TestMethod]
////            public void HandlesGeneralException()
////            {
////                // Arrange.
////                this.mockMerchantsRepository
////                    .Setup(m => m.GetMerchant(It.IsAny<long>()))
////                    .Throws(new Exception());

////                // Act.
////                MerchantsController controller = new MerchantsController(this.mockMerchantsRepository.Object);
////                IActionResult result = controller.Get(id);
////                ObjectResult objectResult = result as ObjectResult;

////                // Assert.
////                Assert.IsNotNull(objectResult, "An object result should have been returned.");
////                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
////                string expectedMessage = "There was an error getting the merchant.";
////                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
////            }
////        }
////    }
////}
