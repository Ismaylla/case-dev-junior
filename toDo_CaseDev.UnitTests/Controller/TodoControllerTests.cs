using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Services;

namespace toDo_CaseDev.UnitTests.Controller
{
    public class TodoControllerTests
    {
        private readonly Mock<ITodoService> _mockTodoService;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _mockTodoService = new Mock<ITodoService>();
            _controller = new TodoController(_mockTodoService.Object);
        }

        [Fact]
        public void GetAll_ReturnsListOfTodos()
        {
            // Arrange
            var expectedTodos = new List<TodoItemDto>
            {
                new TodoItemDto { Id = 1, Title = "Teste Todo 1", Description = "Descrição Teste Todo 1 ", Status = TodoStatus.Pendente.ToString() },
                new TodoItemDto { Id = 2, Title = "Teste Todo 2", Description = "Descrição Teste Todo 2", Status = TodoStatus.EmAndamento.ToString()},
                new TodoItemDto { Id = 3, Title = "Teste Todo 3", Description = "Descrição Teste Todo 3", Status = TodoStatus.Concluida.ToString()}
            };
            _mockTodoService.Setup(service => service.GetAll()).Returns(expectedTodos);

            // Act
            var result = _controller.GetAll();

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<TodoItemDto>>>(result);
            var returnValue = Assert.IsType<List<TodoItemDto>>(actionResult.Value);
            Assert.Equal(expectedTodos, returnValue);
        }

        [Fact]
        public void Create_WithValidTodo_ReturnsCreatedResponse()
        {
            //Arrange
            var todoToCreate = new TodoItem { Title = "New Todo", Description = "Test Description" };
            var createdTodo = new TodoItem { Id = 1, Title = "New Todo", Description = "Test Description" };
            _mockTodoService.Setup(service => service.Create(It.IsAny<TodoItem>())).Returns(createdTodo);

            //  Act
            var result = _controller.Create(todoToCreate);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal(nameof(_controller.GetAll), createdAtActionResult.ActionName);
            Assert.Equal(createdTodo.Id, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public void Create_WithInvalidTodo_ReturnsBadRequest()
        {
            //Arrange
            var invalidTodo = new TodoItem { Title = string.Empty, Description = "Test Description" };

            //Act
            var result = _controller.Create(invalidTodo);

            //Assert
            var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public void UpdateStatus_WithValidId_ReturnsNoContent()
        {
            //  Arrange
            int todoId = 1;
            var existingTodo = new TodoItem { Id = todoId, Title = "Teste Todo", Description = "Test Description" };
            var newStatus = TodoStatus.Concluida;
            _mockTodoService.Setup(service => service.GetById(todoId)).Returns(existingTodo);

            // Act
            var result = _controller.UpdateStatus(todoId, newStatus);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockTodoService.Verify(service => service.UpdateStatus(todoId, newStatus), Times.Once);
        }

        [Fact]
        public void UpdateStatus_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int todoId = 999;
            _mockTodoService.Setup(service => service.GetById(todoId)).Returns((TodoItem?)null);

            // Act
            var result = _controller.UpdateStatus(todoId, TodoStatus.Concluida);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            int todoId = 1;
            var existingTodo = new TodoItem { Id = todoId, Title = "Teste Todo", Description = "Test Description" };
            _mockTodoService.Setup(service => service.GetById(todoId)).Returns(existingTodo);

            // Act
            var result = _controller.Delete(todoId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockTodoService.Verify(service => service.Delete(todoId), Times.Once);
        }

        [Fact]
        public void Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            int todoId = 999;
            _mockTodoService.Setup(service => service.GetById(todoId)).Returns((TodoItem?)null);

            // Act
            var result = _controller.Delete(todoId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}