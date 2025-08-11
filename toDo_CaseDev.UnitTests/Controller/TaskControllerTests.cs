using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskApi.Controllers;
using TaskApi.Exceptions;
using TaskApi.Models;
using TaskApi.Services;
using TaskStatus = TaskApi.Models.TaskStatus;

namespace TaskApi.UnitTests.Controller
{
    public class TodoControllerTests
    {
        private readonly Mock<ITaskService> _mockTodoService;
        private readonly TodoController _controller;

        public TodoControllerTests()
        {
            _mockTodoService = new Mock<ITaskService>();
            _controller = new TodoController(_mockTodoService.Object);
        }

        #region GetAll Tests
        [Fact]
        public void GetAll_ReturnsOkResultWithTaskDtos()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new() { Id = 1, Title = "Task 1", Description = "Descrição 1", Status = TaskStatus.Pendente },
                new() { Id = 2, Title = "Task 2", Description = "Descrição 2", Status = TaskStatus.EmAndamento }
            };
            _mockTodoService.Setup(service => service.GetAll()).Returns(tasks);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TaskDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
            Assert.Equal("Task 1", returnValue[0].Title);
            Assert.Equal(TaskStatus.Pendente, returnValue[0].Status);
            Assert.Equal("Task 2", returnValue[1].Title);
            Assert.Equal(TaskStatus.EmAndamento, returnValue[1].Status);
        }

        [Fact]
        public void GetAll_WhenEmpty_ReturnsEmptyList()
        {
            // Arrange
            var tasks = new List<TaskItem>();
            _mockTodoService.Setup(service => service.GetAll()).Returns(tasks);

            // Act
            var result = _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TaskDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        [Fact]
        public void GetAll_WhenExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            _mockTodoService.Setup(service => service.GetAll()).Throws(new Exception("Erro de dados"));

            // Act
            var result = _controller.GetAll();

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("Erro Interno", errorResponse.Status);
            Assert.Contains("Erro de dados", errorResponse.Mensagens);
        }
        #endregion

        #region GetById Tests
        [Fact]
        public void GetById_WithValidId_ReturnsOkResultWithTaskDto()
        {
            // Arrange
            var task = new TaskItem 
            { 
                Id = 1, 
                Title = "Task Teste", 
                Description = "Descrição da Task",
                Status = TaskStatus.Pendente 
            };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(task);

            // Act
            var result = _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
            Assert.Equal("Task Teste", returnValue.Title);
            Assert.Equal("Descrição da Task", returnValue.Description);
            Assert.Equal(TaskStatus.Pendente, returnValue.Status);
        }

        [Fact]
        public void GetById_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TaskItem?)null);

            // Act
            var result = _controller.GetById(99);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
            Assert.Contains("A tarefa com o ID fornecido não existe.", errorResponse.Mensagens);
        }

        [Fact]
        public void GetById_WhenExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            _mockTodoService.Setup(service => service.GetById(1)).Throws(new Exception("Erro de dados"));

            // Act
            var result = _controller.GetById(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("Erro Interno", errorResponse.Status);
            Assert.Contains("Erro de dados", errorResponse.Mensagens);
        }
        #endregion

        #region Create Tests
        [Fact]
        public void Create_WithValidTask_ReturnsCreatedAtAction()
        {
            // Arrange
            var createDto = new CreateTaskDto 
            { 
                Title = "Nova Task",
                Description = "Nova Descrição"
            };
            var createdTask = new TaskItem 
            { 
                Id = 1, 
                Title = "Nova Task", 
                Description = "Nova Descrição", 
                Status = TaskStatus.Pendente 
            };
            _mockTodoService.Setup(service => service.Create(It.Is<TaskItem>(t => 
                t.Title == createDto.Title && 
                t.Description == createDto.Description)))
                .Returns(createdTask);

            // Act
            var result = _controller.Create(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues?["id"]);
            
            var returnValue = Assert.IsType<TaskItem>(createdAtActionResult.Value);
            Assert.Equal("Nova Task", returnValue.Title);
            Assert.Equal("Nova Descrição", returnValue.Description);
            Assert.Equal(TaskStatus.Pendente, returnValue.Status);
        }

        [Fact]
        public void Create_WithNullDescription_UsesEmptyString()
        {
            // Arrange
            var createDto = new CreateTaskDto 
            { 
                Title = "Nova Task",
                Description = null
            };
            var createdTask = new TaskItem 
            { 
                Id = 1, 
                Title = "Nova Task", 
                Description = string.Empty, 
                Status = TaskStatus.Pendente 
            };
            
            _mockTodoService.Setup(service => service.Create(It.Is<TaskItem>(t => 
                t.Title == "Nova Task" && 
                t.Description == string.Empty)))
                .Returns(createdTask);

            // Act
            var result = _controller.Create(createDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<TaskItem>(createdAtActionResult.Value);
            Assert.Equal(string.Empty, returnValue.Description);
        }

        [Fact]
        public void Create_WhenValidationFails_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new CreateTaskDto 
            { 
                Title = "",
                Description = "Nova Descrição"
            };
            _mockTodoService.Setup(service => service.Create(It.IsAny<TaskItem>()))
                .Throws(new ArgumentException("O título da tarefa é obrigatório."));
            
            _controller.ModelState.AddModelError("Title", "O título da tarefa é obrigatório.");

            // Act
            var result = _controller.Create(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Create_WhenServiceThrows_ReturnsBadRequest()
        {
            // Arrange
            var createDto = new CreateTaskDto { Title = "Nova Task", Description = "Nova Descrição" };
            _mockTodoService.Setup(service => service.Create(It.IsAny<TaskItem>()))
                .Throws(new Exception("Erro ao criar tarefa"));

            // Act
            var result = _controller.Create(createDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("Erro ao criar tarefa", errorResponse.Status);
            Assert.Contains("Erro ao criar tarefa", errorResponse.Mensagens);
        }
        #endregion

        #region Update Tests
        [Fact]
        public void Update_WithValidIdAndData_ReturnsOkResult()
        {
            // Arrange
            var updateDto = new CreateTaskDto 
            { 
                Title = "Task Atualizada",
                Description = "Descrição Atualizada"
            };
            var existingTask = new TaskItem
            {
                Id = 1,
                Title = "Task Original",
                Description = "Descrição Original",
                Status = TaskStatus.Pendente
            };
            var updatedTask = new TaskItem
            {
                Id = 1,
                Title = "Task Atualizada",
                Description = "Descrição Atualizada",
                Status = TaskStatus.Pendente
            };

            _mockTodoService.Setup(service => service.GetById(1)).Returns(existingTask);
            _mockTodoService.Setup(service => service.Update(It.IsAny<TaskItem>()))
                .Returns(updatedTask);

            // Act
            var result = _controller.Update(1, updateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal(updateDto.Title, returnValue.Title);
            Assert.Equal(updateDto.Description, returnValue.Description);
        }

        [Fact]
        public void Update_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var updateDto = new CreateTaskDto 
            { 
                Title = "Tarefa Atualizada",
                Description = "Descrição Atualizada"
            };
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TaskItem?)null);

            // Act
            var result = _controller.Update(99, updateDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
            Assert.Contains("A tarefa com o ID fornecido não existe.", errorResponse.Mensagens);
        }

        [Fact]
        public void Update_WhenValidationFails_ReturnsBadRequest()
        {
            // Arrange
            var updateDto = new CreateTaskDto 
            { 
                Title = "",
                Description = "Descrição Atualizada"
            };
            _controller.ModelState.AddModelError("Title", "O título da tarefa é obrigatório.");

            // Act
            var result = _controller.Update(1, updateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Update_WhenServiceThrows_ReturnsBadRequest()
        {
            // Arrange
            var updateDto = new CreateTaskDto 
            { 
                Title = "Tarefa Atualizada",
                Description = "Descrição Atualizada"
            };
            var existingTask = new TaskItem
            {
                Id = 1,
                Title = "Task Original",
                Description = "Descrição Original"
            };

            _mockTodoService.Setup(service => service.GetById(1)).Returns(existingTask);
            _mockTodoService.Setup(service => service.Update(It.IsAny<TaskItem>()))
                .Throws(new Exception("Erro ao atualizar tarefa"));

            // Act
            var result = _controller.Update(1, updateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("Erro ao atualizar tarefa", errorResponse.Status);
            Assert.Contains("Erro ao atualizar tarefa", errorResponse.Mensagens);
        }
        #endregion

        #region UpdateStatus Tests
        [Fact]
         public void UpdateStatus_WithValidId_ReturnsOkResult()
        {
            // Arrange
            var taskId = 1;
            var existingTask = new TaskItem
            {
                Id = taskId,
                Title = "Task",
                Description = "Descrição",
                Status = TaskStatus.Pendente
            };
            var updatedTask = new TaskItem
            {
                Id = taskId,
                Title = "Task",
                Description = "Descrição",
                Status = TaskStatus.EmAndamento
            };
            var statusDto = new UpdateTaskStatusDto { Status = TaskStatus.EmAndamento };

            // Setup initial GetById
            _mockTodoService.Setup(service => service.GetById(taskId))
                .Returns(existingTask);
            
            // Setup UpdateStatus
            _mockTodoService.Setup(service => service.UpdateStatus(taskId, statusDto.Status));

            // Setup second GetById that returns the updated task
            _mockTodoService.SetupSequence(service => service.GetById(taskId))
                .Returns(existingTask)    
                .Returns(updatedTask);   

            // Act
            var result = _controller.UpdateStatus(taskId, statusDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(TaskStatus.EmAndamento, returnValue.Status);
        }

        [Fact]
        public void UpdateStatus_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var statusDto = new UpdateTaskStatusDto { Status = TaskStatus.EmAndamento };
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TaskItem?)null);

            // Act
            var result = _controller.UpdateStatus(99, statusDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
            Assert.Contains("A tarefa com o ID fornecido não existe.", errorResponse.Mensagens);
        }

        [Fact]
        public void UpdateStatus_WhenServiceThrows_ReturnsBadRequest()
        {
            // Arrange
            var taskId = 1;
            var statusDto = new UpdateTaskStatusDto { Status = TaskStatus.EmAndamento };
            var existingTask = new TaskItem
            {
                Id = taskId,
                Title = "Task",
                Description = "Descrição",
                Status = TaskStatus.Pendente
            };

            _mockTodoService.Setup(service => service.GetById(taskId)).Returns(existingTask);
            _mockTodoService.Setup(service => service.UpdateStatus(taskId, statusDto.Status))
                .Throws(new Exception("Erro ao atualizar status"));

            // Act
            var result = _controller.UpdateStatus(taskId, statusDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("Erro ao atualizar status", errorResponse.Status);
            Assert.Contains("Erro ao atualizar status", errorResponse.Mensagens);
        }
        #endregion

        #region Delete Tests
        [Fact]
        public void Delete_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var taskId = 1;
            var existingTask = new TaskItem
            {
                Id = taskId,
                Title = "Task",
                Description = "Descrição"
            };
            _mockTodoService.Setup(service => service.GetById(taskId)).Returns(existingTask);

            // Act
            var result = _controller.Delete(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockTodoService.Verify(service => service.Delete(taskId), Times.Once);
        }

        [Fact]
        public void Delete_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TaskItem?)null);

            // Act
            var result = _controller.Delete(99);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
            Assert.Contains("A tarefa com o ID fornecido não existe.", errorResponse.Mensagens);
        }

        [Fact]
        public void Delete_WhenServiceThrows_ReturnsBadRequest()
        {
            // Arrange
            var taskId = 1;
            var existingTask = new TaskItem
            {
                Id = taskId,
                Title = "Task",
                Description = "Descrição"
            };
            _mockTodoService.Setup(service => service.GetById(taskId)).Returns(existingTask);
            _mockTodoService.Setup(service => service.Delete(taskId))
                .Throws(new Exception("Erro ao deletar tarefa"));

            // Act
            var result = _controller.Delete(taskId);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(badRequestResult.Value);
            Assert.Equal("Erro ao deletar tarefa", errorResponse.Status);
            Assert.Contains("Erro ao deletar tarefa", errorResponse.Mensagens);
        }
        #endregion
    }
}
