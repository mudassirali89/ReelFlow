using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReelFlow.DTO;
using ReelFlow.Models;
using ReelFLow.Controllers;
using ReelFLow.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReelFlowTests
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryRepository> _mockCategoryRepo;
        private CategoryController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockCategoryRepo = new Mock<ICategoryRepository>();
            _controller = new CategoryController(_mockCategoryRepo.Object);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ReturnsOkResultWithCategoryList()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Action", Description = "Action movies" },
                new Category { Id = 2, Name = "Comedy", Description = "Comedy movies" }
            };

            _mockCategoryRepo.Setup(repo => repo.GetAllCategoriesAsync())
                            .ReturnsAsync(categories);

            var result = await _controller.GetAllCategoriesAsync();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var returnedCategories = okResult.Value as IEnumerable<Category>;
            Assert.That(returnedCategories, Is.EqualTo(categories));
        }
        [Test]
        public async Task GetCategoryByIdAsync_ValidId_ReturnsOkResultWithCategory()
        {
            var categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Action", Description = "Action movies" };

            _mockCategoryRepo.Setup(repo => repo.GetCategoryByIdAsync(categoryId))
                            .ReturnsAsync(category);

            var result = await _controller.GetCategoryByIdAsync(categoryId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            var returnedCategory = okResult.Value as Category;

            Assert.That(returnedCategory, Is.EqualTo(category));
        }
        [Test]
        public async Task GetCategoryByIdAsync_InvalidId_ReturnsNotFoundResult()
        {
            var invalidId = 999;

            _mockCategoryRepo.Setup(repo => repo.GetCategoryByIdAsync(invalidId))
                    .ReturnsAsync((Category?)null);

            var result = await _controller.GetCategoryByIdAsync(invalidId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = (NotFoundObjectResult)result;
            Assert.That(notFoundResult.Value, Is.EqualTo("Category not found."));
        }
        [Test]
        public async Task UpdateCategoryAsync_ValidDto_ReturnsOkResultWithString()
        {
            var categoryId = 1;
            var updatedCategoryDto = new CategoryDTO { Name = "Updated Action", Description = "Updated Action movies" };
            var existingCategory = new Category { Id = categoryId, Name = "Action", Description = "Action movies" };
    
            _mockCategoryRepo.Setup(repo => repo.GetCategoryByIdAsync(categoryId))
                     .ReturnsAsync(existingCategory);

            _mockCategoryRepo.Setup(repo => repo.UpdateCategoryAsync(existingCategory))
                     .ReturnsAsync(true);

            var result = await _controller.UpdateCategoryAsync(categoryId, updatedCategoryDto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;

            Assert.That(okResult?.Value, Is.EqualTo("Category updated successfully."));
    
            _mockCategoryRepo.Verify(repo => repo.GetCategoryByIdAsync(categoryId), Times.Once);
            _mockCategoryRepo.Verify(repo => repo.UpdateCategoryAsync(existingCategory), Times.Once);
    
            Assert.That(existingCategory.Name, Is.EqualTo(updatedCategoryDto.Name));
            Assert.That(existingCategory.Description, Is.EqualTo(updatedCategoryDto.Description));
        }

        [Test]
        public async Task UpdateCategoryAsync_InvalidId_ReturnsNotFoundResultWithString()
        {
            var invalidId = 999;
            var updatedCategoryDto = new CategoryDTO { Name = "Invalid Update", Description = "" };
    
            _mockCategoryRepo.Setup(repo => repo.GetCategoryByIdAsync(invalidId))
                     .ReturnsAsync((Category?)null);

            var result = await _controller.UpdateCategoryAsync(invalidId, updatedCategoryDto);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
    
            Assert.That(notFoundResult?.Value, Is.EqualTo("Category not found."));

            _mockCategoryRepo.Verify(repo => repo.UpdateCategoryAsync(It.IsAny<Category>()), Times.Never);
        }

        [Test]
        public async Task AddNewCategoryAsync_CategoryAlreadyExists_ReturnsBadRequest()
        {
            var categoryDTO = new CategoryDTO { Name = "Action", Description = "Action movies" };
            var existingCategory = new Category { Id = 1, Name = "Action" };

            _mockCategoryRepo.Setup(repo => repo.CategoryExistsAsync(categoryDTO.Name)).ReturnsAsync(existingCategory);

            var result = await _controller.AddNewCategoryAsync(categoryDTO);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.Value, Is.EqualTo("category already exists"));
        }

        [Test]
        public async Task AddNewCategoryAsync_ValidCategory_ReturnsOk()
        {
            var categoryDTO = new CategoryDTO { Name = "Horror", Description = "Scary movies" };

            _mockCategoryRepo.Setup(repo => repo.CategoryExistsAsync(categoryDTO.Name)).ReturnsAsync((Category)null);
            _mockCategoryRepo.Setup(repo => repo.CreateCategoryAsync(It.IsAny<Category>())).ReturnsAsync(new Category());

            var result = await _controller.AddNewCategoryAsync(categoryDTO);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo("Category created successfully"));
        }

        [Test]
        public async Task DeleteCategoryAsync_CategoryNotFound_ReturnsNotFound()
        {
            var categoryId = 999;

            _mockCategoryRepo.Setup(repo => repo.GetCategoryByIdAsync(categoryId))
                             .ReturnsAsync((Category?)null);

            var result = await _controller.DeleteCategoryAsync(categoryId);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.That(notFoundResult?.Value, Is.EqualTo("Category not found."));

            _mockCategoryRepo.Verify(repo => repo.DeleteCategoryAsync(It.IsAny<Category>()), Times.Never);
        }

        [Test]
        public async Task DeleteCategoryAsync_CategoryHasMovies_ReturnsBadRequest()
        {
            var categoryId = 1;
            var existingCategory = new Category { Id = categoryId, Name = "Action" };

            _mockCategoryRepo.Setup(repo => repo.GetCategoryByIdAsync(categoryId))
                             .ReturnsAsync(existingCategory);

            _mockCategoryRepo.Setup(repo => repo.DeleteCategoryAsync(existingCategory))
                             .ReturnsAsync(false);

            var result = await _controller.DeleteCategoryAsync(categoryId);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult?.Value, Is.EqualTo("Cannot delete category because it has associated movies."));

            _mockCategoryRepo.Verify(repo => repo.DeleteCategoryAsync(existingCategory), Times.Once);
        }

        [Test]
        public async Task DeleteCategoryAsync_CategoryDeletedSuccessfully_ReturnsOk()
        {
            var categoryId = 2;
            var existingCategory = new Category { Id = categoryId, Name = "Comedy" };

            _mockCategoryRepo.Setup(repo => repo.GetCategoryByIdAsync(categoryId))
                             .ReturnsAsync(existingCategory);

            _mockCategoryRepo.Setup(repo => repo.DeleteCategoryAsync(existingCategory))
                             .ReturnsAsync(true);

            var result = await _controller.DeleteCategoryAsync(categoryId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult?.Value, Is.EqualTo("Category deleted successfully."));

            _mockCategoryRepo.Verify(repo => repo.DeleteCategoryAsync(existingCategory), Times.Once);
        }
    }
}
