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
    /// Tests for the <see cref="CategoriesController"/> class.
    /// </summary>
    public class CategoriesControllerTests
    {
        /// <summary>
        /// The mock implementation of the categories repository.
        /// </summary>
        private Mock<ICategoriesRepository> mockCategoriesRepository;

        /// <summary>
        /// Initializes the tests for the class.
        /// </summary>
        [TestInitialize]
        public virtual void Initialize()
        {
            // Initialize the mock object(s).
            this.mockCategoriesRepository = new Mock<ICategoriesRepository>();
        }

        /// <summary>
        /// Tests for the parameterless Get() method.
        /// </summary>
        [TestClass]
        public class GetMethod : CategoriesControllerTests
        {
            /// <summary>
            /// The user ID.
            /// </summary>
            private long userId;

            /// <summary>
            /// The stub repository response.
            /// </summary>
            private List<Category> stubCategories;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                this.userId = 1;

                // Initialize the mock repository method.
                this.stubCategories = new List<Category>();
                this.mockCategoriesRepository
                    .Setup(m => m.GetAll(It.IsAny<long>()))
                    .Returns(this.stubCategories);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Get();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockCategoriesRepository.Verify(m => m.GetAll(this.userId), Times.Once, "The categories should have been requested from the repository.");
                this.mockCategoriesRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubCategories, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies an empty list is returned when the repository returns null.
            /// </summary>
            [TestMethod]
            public void ReturnsAnEmptyListWhenRepositoryReturnsNull()
            {
                // Arrange.
                this.stubCategories = null;
                this.mockCategoriesRepository
                    .Setup(m => m.GetAll(It.IsAny<long>()))
                    .Returns(this.stubCategories);

                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Get();
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockCategoriesRepository.Verify(m => m.GetAll(this.userId), Times.Once, "The categories should have been requested from the repository.");
                this.mockCategoriesRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(0, (okResult.Value as List<Category>).Count, "An empty list should be the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockCategoriesRepository
                    .Setup(m => m.GetAll(It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Get();
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error getting the categories.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }
        }

        /// <summary>
        /// Tests for the Get() method that takes in an ID value.
        /// </summary>
        [TestClass]
        public class GetMethod_Id : CategoriesControllerTests
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
            private Category stubCategory;

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
                this.stubCategory = new Category();
                this.mockCategoriesRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns(this.stubCategory);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Get(this.id);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockCategoriesRepository.Verify(m => m.Get(this.id, this.userId), Times.Once, "The categories should have been requested from the repository.");
                this.mockCategoriesRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubCategory, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies an empty list is returned when the repository returns null.
            /// </summary>
            [TestMethod]
            public void ReturnsNotFoundWhenRepositoryReturnsNull()
            {
                // Arrange.
                this.stubCategory = null;
                this.mockCategoriesRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns(this.stubCategory);

                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Get(this.id);
                NotFoundResult notFoundResult = result as NotFoundResult;

                // Assert.
                this.mockCategoriesRepository.Verify(m => m.Get(this.id, this.userId), Times.Once, "The category should have been requested from the repository.");
                this.mockCategoriesRepository.VerifyNoOtherCalls();

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
                this.mockCategoriesRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Returns<Category>(null);

                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Get(this.id);
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
                this.mockCategoriesRepository
                    .Setup(m => m.Get(It.IsAny<long>(), It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Get(this.id);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error getting the category.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }
        }

        /// <summary>
        /// Tests for the Post() method.
        /// </summary>
        [TestClass]
        public class PostMethod : CategoriesControllerTests
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
            /// Stub of the category returned by the repository.
            /// </summary>
            private Category stubCategory;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                this.userId = 1;
                this.category = new Category();

                // Initialize the mock repository method.
                this.stubCategory = new Category();
                this.mockCategoriesRepository
                    .Setup(m => m.Add(It.IsAny<Category>(), It.IsAny<long>()))
                    .Returns(this.stubCategory);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Post(this.category);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockCategoriesRepository.Verify(m => m.Add(this.category, this.userId), Times.Once, "The add method should have been called.");
                this.mockCategoriesRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubCategory, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsBadRequestErrorWhenCategoryNull()
            {
                // Arrange.
                this.category = null;

                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Post(this.category);
                BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;

                // Assert.
                this.mockCategoriesRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(badRequestResult, "A bad request response should have been returned.");
                Assert.AreEqual(400, badRequestResult.StatusCode, "The status code from the response should have been 405.");
                string expectedMessage = "A category must be passed in for it to be saved.";
                Assert.AreEqual(expectedMessage, badRequestResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockCategoriesRepository
                    .Setup(m => m.Add(It.IsAny<Category>(), It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Post(this.category);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error saving the category.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies the User ID gets added to the entity before using the repository.
            /// </summary>
            [TestMethod]
            public void AddsUserIdToEntity()
            {
                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Post(this.category);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockCategoriesRepository.Verify(m => m.Add(It.Is<Category>(x => x.UserId == this.userId), It.IsAny<long>()), Times.Once, "The user ID should have been added to the entity.");
            }
        }

        /// <summary>
        /// Tests for the Put() method.
        /// </summary>
        [TestClass]
        public class PutMethod : CategoriesControllerTests
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
            /// The category used as an input.
            /// </summary>
            private Category category;

            /// <summary>
            /// Stub of the category returned by the repository.
            /// </summary>
            private Category stubCategory;

            /// <summary>
            /// Initializes the tests for the method.
            /// </summary>
            [TestInitialize]
            public override void Initialize()
            {
                base.Initialize();

                this.id = 7;
                this.category = new Category
                {
                    Id = id,
                };
                this.userId = 1;

                // Initialize the mock repository method.
                this.stubCategory = new Category();
                this.mockCategoriesRepository
                    .Setup(m => m.Save(It.IsAny<Category>(), It.IsAny<long>()))
                    .Returns(this.stubCategory);
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsRepositoryResult()
            {
                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Put(this.id, this.category);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockCategoriesRepository.Verify(m => m.Save(this.category, this.userId), Times.Once, "The save method should have been called.");
                this.mockCategoriesRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(okResult, "An OK response should have been returned.");
                Assert.AreEqual(200, okResult.StatusCode, "The status code from the response should have been 200.");
                Assert.AreEqual(this.stubCategory, okResult.Value, "The result from the repository should have been returned.");
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsBadRequestErrorWhenCategoryNull()
            {
                // Arrange.
                this.category = null;

                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Put(this.id, this.category);
                BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;

                // Assert.
                this.mockCategoriesRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(badRequestResult, "A bad request response should have been returned.");
                Assert.AreEqual(400, badRequestResult.StatusCode, "The status code from the response should have been 405.");
                string expectedMessage = "A category must be passed in for it to be saved.";
                Assert.AreEqual(expectedMessage, badRequestResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies the result from the repository is retrieved correctly.
            /// </summary>
            [TestMethod]
            public void ReturnsBadRequestErrorWhenCategoryIdsMismatch()
            {
                // Arrange.
                this.id = this.id + 2;

                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Put(this.id, this.category);
                BadRequestObjectResult badRequestResult = result as BadRequestObjectResult;

                // Assert.
                this.mockCategoriesRepository.VerifyNoOtherCalls();

                Assert.IsNotNull(badRequestResult, "A bad request response should have been returned.");
                Assert.AreEqual(400, badRequestResult.StatusCode, "The status code from the response should have been 405.");
                string expectedMessage = "The category ID values did not match.";
                Assert.AreEqual(expectedMessage, badRequestResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies that general exceptions are handled correctly.
            /// </summary>
            [TestMethod]
            public void HandlesGeneralException()
            {
                // Arrange.
                this.mockCategoriesRepository
                    .Setup(m => m.Save(It.IsAny<Category>(), It.IsAny<long>()))
                    .Throws(new Exception());

                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Put(this.id, this.category);
                ObjectResult objectResult = result as ObjectResult;

                // Assert.
                Assert.IsNotNull(objectResult, "An object result should have been returned.");
                Assert.AreEqual(500, objectResult.StatusCode, "The status code from the response should have been 500.");
                string expectedMessage = "There was an error saving the category.";
                Assert.AreEqual(expectedMessage, objectResult.Value, "The error message should have been the result.");
            }

            /// <summary>
            /// Verifies the User ID gets added to the entity before using the repository.
            /// </summary>
            [TestMethod]
            public void AddsUserIdToEntity()
            {
                // Act.
                CategoriesController controller = new CategoriesController(this.mockCategoriesRepository.Object);
                IActionResult result = controller.Put(this.id, this.category);
                OkObjectResult okResult = result as OkObjectResult;

                // Assert.
                this.mockCategoriesRepository.Verify(m => m.Save(It.Is<Category>(x => x.UserId == this.userId), It.IsAny<long>()), Times.Once, "The user ID should have been added to the entity.");
            }
        }
    }
}
