using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaskApi.Controllers;
using TaskApi.Exceptions;
using TaskApi.Models;
using TaskApi.Services;
using TaskStatus = TaskApi.Models.TaskStatus;

namespace TaskApi.UnitTests.Controller {
    public class TaskControllerTests {
        private readonly Mock<ITaskService> _mockTodoService;
        private readonly Mock<ILogger<TaskController>> _mockLogger;
        private readonly TaskController _controller;

        public TaskControllerTests() {
            _mockTodoService = new Mock<ITaskService>();
            _mockLogger = new Mock<ILogger<TaskController>>();
            _controller = new TaskController(_mockTodoService.Object, _mockLogger.Object);
        }

        #region GetAll Tests
        [Fact]
        public void GetAll_ReturnsOkResultWithTaskDtos() {
            var tasks = new List<TaskItem>
            {
                new() { Id = 1, Title = "Task 1", Description = "Descrição 1", Status = TaskStatus.Pendente },
                new() { Id = 2, Title = "Task 2", Description = "Descrição 2", Status = TaskStatus.EmAndamento }
            };
            _mockTodoService.Setup(service => service.GetAll()).Returns(tasks);

            var result = _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TaskDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public void GetAll_WhenEmpty_ReturnsEmptyList() {
            _mockTodoService.Setup(service => service.GetAll()).Returns(new List<TaskItem>());

            var result = _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<TaskDto>>(okResult.Value);
            Assert.Empty(returnValue);
        }

        [Fact]
        public void GetAll_WhenExceptionThrown_ReturnsInternalServerError() {
            _mockTodoService.Setup(service => service.GetAll()).Throws(new Exception("Erro de dados"));

            var result = _controller.GetAll();

            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            var errorResponse = Assert.IsType<ApiErrorResponse>(objectResult.Value);
            Assert.Equal("Erro Interno", errorResponse.Status);
            Assert.Contains("Ocorreu um erro ao processar sua solicitação.", errorResponse.Mensagens);
        }
        #endregion

        #region GetById Tests
        [Fact]
        public void GetById_WithValidId_ReturnsOkResultWithTaskDto() {
            var task = new TaskItem { Id = 1, Title = "Task Teste", Description = "Descrição da Task", Status = TaskStatus.Pendente };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(task);

            var result = _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void GetById_WithInvalidId_ReturnsNotFound() {
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TaskItem?)null);

            var result = _controller.GetById(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var errorResponse = Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
            Assert.Equal("Recurso Não Encontrado", errorResponse.Status);
        }

        [Fact]
        public void GetById_WhenExceptionThrown_ReturnsInternalServerError() {
            _mockTodoService.Setup(service => service.GetById(1)).Throws(new Exception("Erro de dados"));

            var result = _controller.GetById(1);

            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            var errorResponse = Assert.IsType<ApiErrorResponse>(objectResult.Value);
            Assert.Equal("Erro Interno", errorResponse.Status);
        }
        #endregion

        #region Create Tests
        [Fact]
        public void Create_WithValidTask_ReturnsCreatedAtAction() {
            var createDto = new CreateTaskDto { Title = "Nova Task", Description = "Nova Descrição" };
            var createdTask = new TaskItem { Id = 1, Title = "Nova Task", Description = "Nova Descrição", Status = TaskStatus.Pendente };
            _mockTodoService.Setup(service => service.Create(It.IsAny<TaskItem>())).Returns(createdTask);

            var result = _controller.Create(createDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetById), createdAtActionResult.ActionName);
        }

        [Fact]
        public void Create_WhenValidationFails_ReturnsBadRequest() {
            var createDto = new CreateTaskDto { Title = string.Empty, Description = "Descrição" };
            _controller.ModelState.AddModelError("Title", "O título é obrigatório");

            var result = _controller.Create(createDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void Create_WhenServiceThrows_ReturnsInternalServerError() {
            var createDto = new CreateTaskDto { Title = "Nova Task", Description = "Nova Descrição" };
            _mockTodoService.Setup(service => service.Create(It.IsAny<TaskItem>())).Throws(new Exception("Erro ao criar"));

            var result = _controller.Create(createDto);

            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
            var errorResponse = Assert.IsType<ApiErrorResponse>(objectResult.Value);
            Assert.Equal("Erro Interno", errorResponse.Status);
        }
        #endregion

        #region Update Tests
        [Fact]
        public void Update_WithValidIdAndData_ReturnsOkResult() {
            var updateDto = new CreateTaskDto { Title = "Atualizada", Description = "Descrição" };
            var existingTask = new TaskItem { Id = 1, Title = "Antiga", Description = "Antiga" };
            var updatedTask = new TaskItem { Id = 1, Title = "Atualizada", Description = "Descrição" };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(existingTask);
            _mockTodoService.Setup(service => service.Update(existingTask)).Returns(updatedTask);

            var result = _controller.Update(1, updateDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskItem>(okResult.Value);
            Assert.Equal("Atualizada", returnValue.Title);
        }

        [Fact]
        public void Update_WithInvalidId_ReturnsNotFound() {
            var updateDto = new CreateTaskDto { Title = "Atualizada", Description = "Descrição" };
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TaskItem?)null);

            var result = _controller.Update(99, updateDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
        }

        [Fact]
        public void Update_WhenServiceThrows_ReturnsInternalServerError() {
            var updateDto = new CreateTaskDto { Title = "Atualizada", Description = "Descrição" };
            var existingTask = new TaskItem { Id = 1, Title = "Antiga", Description = "Antiga" };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(existingTask);
            _mockTodoService.Setup(service => service.Update(It.IsAny<TaskItem>())).Throws(new Exception("Erro"));

            var result = _controller.Update(1, updateDto);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
        #endregion

        #region UpdateStatus Tests
        [Fact]
        public void UpdateStatus_WithValidId_ReturnsOkResult() {
            var existingTask = new TaskItem { Id = 1, Title = "Task", Description = "Descrição", Status = TaskStatus.Pendente };
            var updatedTask = new TaskItem { Id = 1, Title = "Task", Description = "Descrição", Status = TaskStatus.EmAndamento };
            _mockTodoService.SetupSequence(service => service.GetById(1)).Returns(existingTask).Returns(updatedTask);

            var result = _controller.UpdateStatus(1, new UpdateTaskStatusDto { Status = TaskStatus.EmAndamento });

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<TaskDto>(okResult.Value);
            Assert.Equal(TaskStatus.EmAndamento, returnValue.Status);
        }

        [Fact]
        public void UpdateStatus_WithInvalidId_ReturnsNotFound() {
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TaskItem?)null);

            var result = _controller.UpdateStatus(99, new UpdateTaskStatusDto { Status = TaskStatus.EmAndamento });

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
        }

        [Fact]
        public void UpdateStatus_WhenServiceThrows_ReturnsInternalServerError() {
            var existingTask = new TaskItem { Id = 1, Title = "Task", Description = "Descrição" };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(existingTask);
            _mockTodoService.Setup(service => service.UpdateStatus(1, TaskStatus.EmAndamento)).Throws(new Exception("Erro"));

            var result = _controller.UpdateStatus(1, new UpdateTaskStatusDto { Status = TaskStatus.EmAndamento });

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
        #endregion

        #region Delete Tests
        [Fact]
        public void Delete_WithValidId_ReturnsNoContent() {
            var existingTask = new TaskItem { Id = 1, Title = "Task", Description = "Descrição" };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(existingTask);

            var result = _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_WithInvalidId_ReturnsNotFound() {
            _mockTodoService.Setup(service => service.GetById(99)).Returns((TaskItem?)null);

            var result = _controller.Delete(99);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.IsType<ApiErrorResponse>(notFoundResult.Value);
        }

        [Fact]
        public void Delete_WhenServiceThrows_ReturnsInternalServerError() {
            var existingTask = new TaskItem { Id = 1, Title = "Task", Description = "Descrição" };
            _mockTodoService.Setup(service => service.GetById(1)).Returns(existingTask);
            _mockTodoService.Setup(service => service.Delete(1)).Throws(new Exception("Erro"));

            var result = _controller.Delete(1);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
        }
        #endregion
    }
}
