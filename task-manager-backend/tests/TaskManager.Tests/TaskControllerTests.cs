using Xunit;
using Moq;
using TaskManager.Application.Services;
using TaskManager.Application.DTOs;
using TaskManager.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<TaskService> _mockService;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mockService = new Mock<TaskService>(MockBehavior.Strict, null!);
            _controller = new TasksController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfTasks()
        {
            // Arrange
            var tasks = new List<TaskReadDto>
            {
                new TaskReadDto { Id = 1, Title = "Test", IsCompleted = false, CreatedAt = System.DateTime.UtcNow }
            };

            _mockService.Setup(s => s.GetAllAsync(null, 1, 10)).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAll(null, 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTasks = Assert.IsType<List<TaskReadDto>>(okResult.Value);
            Assert.Single(returnTasks);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((TaskReadDto?)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WhenValid()
        {
            // Arrange
            var dto = new TaskCreateDto { Title = "New Task" };
            var created = new TaskReadDto { Id = 1, Title = "New Task", IsCompleted = false, CreatedAt = System.DateTime.UtcNow };

            _mockService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnTask = Assert.IsType<TaskReadDto>(createdResult.Value);
            Assert.Equal("New Task", returnTask.Title);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenTaskExists()
        {
            // Arrange
            var dto = new TaskUpdateDto { Title = "Updated", IsCompleted = true };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(true);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var dto = new TaskUpdateDto { Title = "Updated", IsCompleted = true };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).ReturnsAsync(false);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenTaskExists()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
