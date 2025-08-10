using Moq;
using TodoApi.Models;
using TodoApi.Repositories;
using TodoApi.Services;

namespace toDo_CaseDev.UnitTests.Service {
    public class TodoServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly TodoService _service;

        public TodoServiceTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _service = new TodoService(_mockRepository.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnAllTodos()
        {
            // Arrange
            var expectedTodos = new List<TodoItemDto>
            {
                new TodoItemDto { Id = 1, Title = "Teste 1", Description = "Desc 1", Status = "Pendente" },
                new TodoItemDto { Id = 2, Title = "Teste 2", Description = "Desc 2", Status = "EmAndamento" }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(expectedTodos);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(expectedTodos, result);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetById_WhenTodoExists_ShouldReturnTodo()
        {
            // Arrange
            var expectedTodo = new TodoItem { Id = 1, Title = "Teste", Description = "Descrição" };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(expectedTodo);

            // Act
            var result = _service.GetById(1);

            // Assert
            Assert.Equal(expectedTodo, result);
            _mockRepository.Verify(repo => repo.GetById(1), Times.Once);
        }

        [Fact]
        public void GetById_WhenTodoDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).Returns((TodoItem?)null);

            // Act
            var result = _service.GetById(1);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(repo => repo.GetById(1), Times.Once);
        }

        [Fact]
        public void Create_WithValidTodo_ShouldCreateAndReturnTodo()
        {
            // Arrange
            var newTodo = new TodoItem { Title = "Teste", Description = "Descrição" };
            var createdTodo = new TodoItem { Id = 1, Title = "Teste", Description = "Descrição" };
            _mockRepository.Setup(repo => repo.Create(It.IsAny<TodoItem>())).Returns(createdTodo);

            // Act
            var result = _service.Create(newTodo);

            // Assert
            Assert.Equal(createdTodo, result);
            _mockRepository.Verify(repo => repo.Create(It.IsAny<TodoItem>()), Times.Once);
        }

        [Fact]
        public void Create_WithEmptyTitle_ShouldThrowArgumentException()
        {
            // Arrange
            var invalidTodo = new TodoItem { Title = string.Empty, Description = "Descrição" };

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _service.Create(invalidTodo));

            // Assert
            Assert.Equal("O título é obrigatório. (Parameter 'Title')", exception.Message);
            _mockRepository.Verify(repo => repo.Create(It.IsAny<TodoItem>()), Times.Never);
        }

        [Fact]
        public void UpdateStatus_WhenTodoExists_ShouldUpdateStatus()
        {
            // Arrange
            var existingTodo = new TodoItem { Id = 1, Title = "Teste", Description = "Descrição" };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(existingTodo);

            // Act
            _service.UpdateStatus(1, TodoStatus.EmAndamento);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateStatus(1, TodoStatus.EmAndamento), Times.Once);
        }

        [Fact]
        public void UpdateStatus_WhenTodoDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).Returns((TodoItem?)null);

            // Act
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _service.UpdateStatus(1, TodoStatus.EmAndamento));

            // Assert
            Assert.Equal("Tarefa com ID 1 não encontrada.", exception.Message);
            _mockRepository.Verify(repo => repo.UpdateStatus(It.IsAny<int>(), It.IsAny<TodoStatus>()), Times.Never);
        }

        [Fact]
        public void Delete_WhenTodoExists_ShouldDeleteTodo()
        {
            // Arrange
            var existingTodo = new TodoItem { Id = 1, Title = "Teste", Description = "Descrição" };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(existingTodo);

            // Act
            _service.Delete(1);

            // Assert
            _mockRepository.Verify(repo => repo.Delete(1), Times.Once);
        }

        [Fact]
        public void Delete_WhenTodoDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).Returns((TodoItem?)null);

            // Act
            var exception = Assert.Throws<KeyNotFoundException>(() => _service.Delete(1));

            // Assert
            Assert.Equal("Tarefa com ID 1 não encontrada.", exception.Message);
            _mockRepository.Verify(repo => repo.Delete(It.IsAny<int>()), Times.Never);
        }
    }
}
