using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.Controllers;
using TodoApi.Models;
using TodoApi.Services;

namespace toDo_CaseDev.UnitTests.Controller {
    public class TodoControllerTests
    {
        private readonly Mock<ITodoService> _mockTodoService;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _mockTodoService = new Mock<ITodoService>();
            _controller = new TodoController(_mockTodoService.Object);
        }

        #region GetAll Tests
        [Fact]
        public void GetAll_ReturnsOkResultWithTodoDtos()
        {
            // Arrange
            var todos = new List<TodoItem>
            {
                new() { Id = 1, Title = "Tarefa 1", Description = "Descrição 1", Status = TodoStatus.Pendente },
                new() { Id = 2, Title = "Tarefa 2", Description = "Descrição 2", Status = TodoStatus.EmAndamento }
            };
            _mockTodoService.Setup(service => service.GetAll()).Returns(todos);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TodoDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal("Tarefa 1", returnValue[0].Title);
            Assert.Equal(TodoStatus.Pendente.GetDescription(), returnValue[0].Status);
            Assert.Equal("Tarefa 2", returnValue[1].Title);
            Assert.Equal(TodoStatus.EmAndamento.GetDescription(), returnValue[1].Status);
        }

        [Fact]
        public void GetAll_WhenEmpty_ReturnsEmptyList()
        {
            // Arrange
            var todos = new List<TodoItem>();
            _mockTodoService.Setup(service => service.GetAll()).Returns(todos);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TodoDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        [Fact]
        public void GetAll_ReturnsAllStatusTypes()
        {
            // Arrange
            var todos = new List<TodoItem>
            {
                new() { Id = 1, Title = "Tarefa 1", Description = "Descrição 1", Status = TodoStatus.Pendente },
                new() { Id = 2, Title = "Tarefa 2", Description = "Descrição 2", Status = TodoStatus.EmAndamento },
                new() { Id = 3, Title = "Tarefa 3", Description = "Descrição 3", Status = TodoStatus.Concluida }
            };
            _mockTodoService.Setup(service => service.GetAll()).Returns(todos);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TodoDto>>(okResult.Value);
            Assert.Equal(3, returnValue.Count);
            Assert.Equal(TodoStatus.Pendente.GetDescription(), returnValue[0].Status);
            Assert.Equal(TodoStatus.EmAndamento.GetDescription(), returnValue[1].Status);
            Assert.Equal(TodoStatus.Concluida.GetDescription(), returnValue[2].Status);
        }

        [Fact]
        public void GetAll_WhenServiceThrows_ReturnsEmptyList()
        {
            // Arrange
            _mockTodoService.Setup(service => service.GetAll()).Returns(new List<TodoItem>());

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TodoDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }
        #endregion

        #region GetById Tests
        [Fact]
        public void GetById_WithValidId_ReturnsOkResultWithTodoDto()
        {
            // Arrange
            var todo = new TodoItem 
            { 
                Id = 1, 
                Title = "Tarefa", 
                Description = "Descrição", 
                Status = TodoStatus.Pendente 
            };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(todo);

            // Act
            var result = _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TodoDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Tarefa", returnValue.Title);
            Assert.Equal("Descrição", returnValue.Description);
            Assert.Equal("Pendente", returnValue.Status);
        }

        [Fact]
        public void GetById_WithInvalidId_ReturnsNotFoundWithErrorResponse()
        {
            // Arrange
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TodoItem?)null);

            // Act
            var result = _controller.GetById(99);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
            Assert.Contains("A tarefa com o ID fornecido não existe.", errorResponse.Mensagens);
        }

        [Fact]
        public void GetById_WhenServiceThrows_ReturnsNotFound()
        {
            // Arrange
            _mockTodoService.Setup(service => service.GetById(1)).Returns((TodoItem?)null);

            // Act
            var result = _controller.GetById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
        }
        #endregion

        #region Create Tests
        [Fact]
        public void Create_WithValidTodo_ReturnsCreatedResponse()
        {
            // Arrange
            var createDto = new CreateTodoDto { Title = "Nova Tarefa", Description = "Nova Descrição" };
            var createdTodo = new TodoItem 
            { 
                Id = 1, 
                Title = "Nova Tarefa", 
                Description = "Nova Descrição", 
                Status = TodoStatus.Pendente 
            };
            _mockTodoService.Setup(service => service.Create(It.Is<TodoItem>(
                t => t.Title == createDto.Title && t.Description == createDto.Description)))
                .Returns(createdTodo);

            // Act
            var result = _controller.Create(createDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal(nameof(_controller.GetAll), createdAtActionResult.ActionName);
            Assert.Equal(createdTodo.Id, createdAtActionResult.RouteValues?["id"]);

            var returnValue = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            Assert.Equal(createDto.Title, returnValue.Title);
            Assert.Equal(createDto.Description, returnValue.Description);
            Assert.Equal(TodoStatus.Pendente, returnValue.Status);
        }

        [Fact]
        public void Create_WithNullDescription_UsesEmptyString()
        {
            // Arrange
            var createDto = new CreateTodoDto { Title = "Nova Tarefa", Description = null };
            var createdTodo = new TodoItem 
            { 
                Id = 1, 
                Title = "Nova Tarefa", 
                Description = string.Empty, 
                Status = TodoStatus.Pendente 
            };
            
            _mockTodoService.Setup(service => service.Create(It.Is<TodoItem>(t => 
                t.Title == "Nova Tarefa" && 
                t.Description == string.Empty)))
                .Returns(createdTodo);

            // Act
            var result = _controller.Create(createDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            Assert.Equal(string.Empty, returnValue.Description);
        }

        [Fact]
        public void Create_WithEmptyDescription_UsesEmptyString()
        {
            // Arrange
            var createDto = new CreateTodoDto { Title = "Nova Tarefa", Description = "" };
            var createdTodo = new TodoItem 
            { 
                Id = 1, 
                Title = "Nova Tarefa", 
                Description = string.Empty, 
                Status = TodoStatus.Pendente 
            };
            
            _mockTodoService.Setup(service => service.Create(It.Is<TodoItem>(t => 
                t.Title == "Nova Tarefa" && 
                t.Description == string.Empty)))
                .Returns(createdTodo);

            // Act
            var result = _controller.Create(createDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<TodoItem>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<TodoItem>(createdAtActionResult.Value);
            Assert.Equal(string.Empty, returnValue.Description);
        }

        [Fact]
        public void Create_WhenServiceFailsValidation_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new CreateTodoDto { Title = "", Description = "Nova Descrição" };

            // Act
            var result = _controller.Create(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
        }
        #endregion

        #region Update Tests
        [Fact]
        public void Update_WithValidIdAndData_ReturnsOkResult()
        {
            // Arrange
            var updateDto = new CreateTodoDto 
            { 
                Title = "Tarefa Atualizada", 
                Description = "Descrição Atualizada" 
            };
            var existingTodo = new TodoItem 
            { 
                Id = 1, 
                Title = "Tarefa Original", 
                Description = "Descrição Original", 
                Status = TodoStatus.Pendente 
            };
            var updatedTodo = new TodoItem 
            { 
                Id = 1, 
                Title = "Tarefa Atualizada", 
                Description = "Descrição Atualizada", 
                Status = TodoStatus.Pendente // Status preserved
            };

            _mockTodoService.Setup(service => service.GetById(1)).Returns(existingTodo);
            _mockTodoService.Setup(service => service.Update(It.IsAny<TodoItem>())).Returns(updatedTodo);

            // Act
            var result = _controller.Update(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal(updateDto.Title, returnValue.Title);
            Assert.Equal(updateDto.Description, returnValue.Description);
            Assert.Equal(TodoStatus.Pendente, returnValue.Status); // Status unchanged
        }

        [Fact]
        public void Update_WithInvalidId_ReturnsNotFoundWithErrorResponse()
        {
            // Arrange
            var updateDto = new CreateTodoDto 
            { 
                Title = "Tarefa Atualizada", 
                Description = "Descrição Atualizada" 
            };
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TodoItem?)null);

            // Act
            var result = _controller.Update(99, updateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
            Assert.Contains("A tarefa com o ID fornecido não existe.", errorResponse.Mensagens);
        }

        [Fact]
        public void Update_WithNullDescription_UsesEmptyString()
        {
            // Arrange
            var updateDto = new CreateTodoDto 
            { 
                Title = "Título Atualizado", 
                Description = null 
            };
            var existingTodo = new TodoItem
            {
                Id = 1,
                Title = "Título da task",
                Description = "Descrição da task",
                Status = TodoStatus.Pendente
            };
            var updatedTodo = new TodoItem
            {
                Id = 1,
                Title = "Título Atualizado",
                Description = string.Empty,
                Status = TodoStatus.Pendente
            };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(existingTodo);
            _mockTodoService.Setup(service => service.Update(It.Is<TodoItem>(t =>
                t.Title == updateDto.Title &&
                t.Description == string.Empty)))
                .Returns(updatedTodo);

            // Act
            var result = _controller.Update(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TodoItem>(okResult.Value);
            Assert.Equal(updateDto.Title, returnValue.Title);
            Assert.Equal(string.Empty, returnValue.Description);
        }

        [Fact]
        public void Update_WhenServiceFailsValidation_ReturnsBadRequest()
        {
            // Arrange
            var updateDto = new CreateTodoDto 
            { 
                Title = string.Empty, // Empty title should fail validation
                Description = "Descrição Atualizada" 
            };
            var existingTodo = new TodoItem
            {
                Id = 1,
                Title = "Título da task",
                Description = "Descrição da task",
                Status = TodoStatus.Pendente
            };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(existingTodo);

            // Act
            var result = _controller.Update(1, updateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
        }
        #endregion

        #region UpdateStatus Tests
        [Fact]
        public void UpdateStatus_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var todoId = 1;
            var existingTodo = new TodoItem 
            { 
                Id = todoId, 
                Title = "Tarefa", 
                Description = "Descrição",
                Status = TodoStatus.Pendente 
            };
            var statusDto = new UpdateStatusTodoDto { Status = TodoStatus.EmAndamento };
            
            _mockTodoService.Setup(service => service.GetById(todoId)).Returns(existingTodo);

            // Act
            var result = _controller.UpdateStatus(todoId, statusDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockTodoService.Verify(service => service.UpdateStatus(todoId, statusDto.Status), Times.Once);
        }

        [Fact]
        public void UpdateStatus_WithInvalidId_ReturnsNotFoundWithErrorResponse()
        {
            // Arrange
            var statusDto = new UpdateStatusTodoDto { Status = TodoStatus.EmAndamento };
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TodoItem?)null);

            // Act
            var result = _controller.UpdateStatus(99, statusDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
            Assert.Contains("A tarefa com o ID fornecido não existe.", errorResponse.Mensagens);
        }

        [Fact]
        public void UpdateStatus_WithInvalidStatus_ReturnsNotFound()
        {
            // Arrange
            var todoId = 1;
            _mockTodoService.Setup(service => service.GetById(todoId)).Returns((TodoItem?)null);
            var statusDto = new UpdateStatusTodoDto { Status = (TodoStatus)999 };

            // Act
            var result = _controller.UpdateStatus(todoId, statusDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
        }

        [Fact]
        public void UpdateStatus_WithSameStatus_ReturnsNoContent()
        {
            // Arrange
            var todoId = 1;
            var currentStatus = TodoStatus.EmAndamento;
            var existingTodo = new TodoItem 
            { 
                Id = todoId, 
                Title = "Tarefa", 
                Description = "Descrição",
                Status = currentStatus 
            };
            var statusDto = new UpdateStatusTodoDto { Status = currentStatus };
            
            _mockTodoService.Setup(service => service.GetById(todoId)).Returns(existingTodo);

            // Act
            var result = _controller.UpdateStatus(todoId, statusDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockTodoService.Verify(service => service.UpdateStatus(todoId, statusDto.Status), Times.Once);
        }
        #endregion

        #region Delete Tests
        [Fact]
        public void Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var todoId = 1;
            var existingTodo = new TodoItem 
            { 
                Id = todoId, 
                Title = "Tarefa", 
                Description = "Descrição" 
            };
            _mockTodoService.Setup(service => service.GetById(todoId)).Returns(existingTodo);

            // Act
            var result = _controller.Delete(todoId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockTodoService.Verify(service => service.Delete(todoId), Times.Once);
        }

        [Fact]
        public void Delete_WithInvalidId_ReturnsNotFoundWithErrorResponse()
        {
            // Arrange
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TodoItem?)null);

            // Act
            var result = _controller.Delete(99);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
            Assert.Contains("A tarefa com o ID fornecido não existe.", errorResponse.Mensagens);
        }

        [Fact]
        public void Delete_WhenServiceDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var todoId = 1;
            _mockTodoService.Setup(service => service.GetById(todoId)).Returns((TodoItem?)null);

            // Act
            var result = _controller.Delete(todoId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
        }
        #endregion
    }
}
