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

        #region GetAll Tests
        [Fact]
        public void GetAll_ShouldReturnAllTodos()
        {
            // Arrange
            var expectedTodos = new List<TodoItem>
            {
                new() { Id = 1, Title = "Tarefa 1", Description = "Descrição 1", Status = TodoStatus.Pendente },
                new() { Id = 2, Title = "Tarefa 2", Description = "Descrição 2", Status = TodoStatus.EmAndamento }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(expectedTodos);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(expectedTodos, result);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetAll_WhenEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var expectedTodos = new List<TodoItem>();
            _mockRepository.Setup(repo => repo.GetAll()).Returns(expectedTodos);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Empty(result);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetAll_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetAll())
                .Throws(new InvalidOperationException("Database error"));

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => _service.GetAll());

            //Assert
            Assert.Equal("Database error", exception.Message);
        }
        #endregion

        #region GetById Tests
        [Fact]
        public void GetById_WhenTodoExists_ShouldReturnTodo()
        {
            // Arrange
            var expectedTodo = new TodoItem
            {
                Id = 1,
                Title = "Tarefa Teste",
                Description = "Descrição Teste",
                Status = TodoStatus.Pendente
            };
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
        public void GetById_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1))
                .Throws(new InvalidOperationException("Database error"));

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => _service.GetById(1));

            //Assert
            Assert.Equal("Database error", exception.Message);
        }
        #endregion

        #region Create Tests
        [Fact]
        public void Create_WithValidTodo_ShouldCreateAndReturnTodo()
        {
            // Arrange
            var newTodo = new TodoItem
            {
                Title = "Nova Tarefa",
                Description = "Nova Descrição"
            };
            var createdTodo = new TodoItem
            {
                Id = 1,
                Title = "Nova Tarefa",
                Description = "Nova Descrição",
                Status = TodoStatus.Pendente
            };
            _mockRepository.Setup(repo => repo.Create(It.IsAny<TodoItem>())).Returns(createdTodo);

            // Act
            var result = _service.Create(newTodo);

            // Assert
            Assert.Equal(createdTodo, result);
            _mockRepository.Verify(repo => repo.Create(It.Is<TodoItem>(t =>
                t.Title == newTodo.Title &&
                t.Description == newTodo.Description &&
                t.Status == TodoStatus.Pendente)), Times.Once);
        }

        [Fact]
        public void Create_WithNullDescription_ShouldCreateWithEmptyDescription()
        {
            // Arrange
            var newTodo = new TodoItem
            {
                Title = "Nova Tarefa",
                Description = null!
            };
            var createdTodo = new TodoItem
            {
                Id = 1,
                Title = "Nova Tarefa",
                Description = string.Empty,
                Status = TodoStatus.Pendente
            };
            _mockRepository.Setup(repo => repo.Create(It.IsAny<TodoItem>())).Returns(createdTodo);

            // Act
            var result = _service.Create(newTodo);

            // Assert
            Assert.Equal(string.Empty, result.Description);
        }

        [Fact]
        public void Create_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            var newTodo = new TodoItem
            {
                Title = "Nova Tarefa",
                Description = "Nova Descrição"
            };
            _mockRepository.Setup(repo => repo.Create(It.IsAny<TodoItem>()))
                .Throws(new InvalidOperationException("Database error"));

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => _service.Create(newTodo));

            //Assert
            Assert.Equal("Database error", exception.Message);
        }
        #endregion

        #region Update Tests
        [Fact]
        public void Update_WithValidTodo_ShouldUpdateAndReturnTodo()
        {
            // Arrange
            var todoToUpdate = new TodoItem
            {
                Id = 1,
                Title = "Tarefa Atualizada",
                Description = "Descrição Atualizada"
            };
            _mockRepository.Setup(repo => repo.Update(It.IsAny<TodoItem>())).Returns(todoToUpdate);

            // Act
            var result = _service.Update(todoToUpdate);

            // Assert
            Assert.Equal(todoToUpdate, result);
            _mockRepository.Verify(repo => repo.Update(It.IsAny<TodoItem>()), Times.Once);
        }

        [Fact]
        public void Update_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var nonExistentTodo = new TodoItem
            {
                Id = 999,
                Title = "Não Existe",
                Description = "Tarefa Inexistente"
            };
            _mockRepository.Setup(repo => repo.Update(It.IsAny<TodoItem>()))
                .Throws(new KeyNotFoundException("Tarefa não encontrada."));

            // Act
            var exception = Assert.Throws<KeyNotFoundException>(() => _service.Update(nonExistentTodo));

            // Assert
            Assert.Equal("Tarefa não encontrada.", exception.Message);
        }

        [Fact]
        public void Update_WithNullDescription_ShouldUpdateWithEmptyDescription()
        {
            // Arrange
            var todoToUpdate = new TodoItem
            {
                Id = 1,
                Title = "Tarefa Atualizada",
                Description = null!
            };
            var updatedTodo = new TodoItem
            {
                Id = 1,
                Title = "Tarefa Atualizada",
                Description = string.Empty
            };
            _mockRepository.Setup(repo => repo.Update(It.IsAny<TodoItem>())).Returns(updatedTodo);

            // Act
            var result = _service.Update(todoToUpdate);

            // Assert
            Assert.Equal(string.Empty, result.Description);
        }
        #endregion

        #region UpdateStatus Tests
        [Fact]
        public void UpdateStatus_WhenTodoExists_ShouldUpdateStatus()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.UpdateStatus(1, TodoStatus.EmAndamento));

            // Act
            _service.UpdateStatus(1, TodoStatus.EmAndamento);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateStatus(1, TodoStatus.EmAndamento), Times.Once);
        }

        [Fact]
        public void UpdateStatus_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.UpdateStatus(1, TodoStatus.EmAndamento))
                .Throws(new KeyNotFoundException("Tarefa não encontrada."));

            // Act & Assert
            var exception = Assert.Throws<KeyNotFoundException>(() =>
                _service.UpdateStatus(1, TodoStatus.EmAndamento));
            Assert.Equal("Tarefa não encontrada.", exception.Message);
        }

        [Fact]
        public void UpdateStatus_WithInvalidStatus_ShouldThrowArgumentException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.UpdateStatus(1, (TodoStatus)999))
                .Throws(new ArgumentException("Status inválido"));

            // Act
            var exception = Assert.Throws<ArgumentException>(() =>
                _service.UpdateStatus(1, (TodoStatus)999));

            // Assert
            Assert.Equal("Status inválido", exception.Message);
        }

        [Fact]
        public void UpdateStatus_WithSameStatus_ShouldUpdateSuccessfully()
        {
            // Arrange
            var existingTodo = new TodoItem
            {
                Id = 1,
                Title = "Tarefa",
                Description = "Descrição",
                Status = TodoStatus.EmAndamento
            };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(existingTodo);
            _mockRepository.Setup(repo => repo.UpdateStatus(1, TodoStatus.EmAndamento));

            // Act
            _service.UpdateStatus(1, TodoStatus.EmAndamento); 
            _mockRepository.Verify(repo => repo.UpdateStatus(1, TodoStatus.EmAndamento), Times.Once);
        }
        #endregion

        #region Delete Tests
        [Fact]
        public void Delete_WhenTodoExists_ShouldDeleteTodo()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Delete(1));

            // Act
            _service.Delete(1);

            // Assert
            _mockRepository.Verify(repo => repo.Delete(1), Times.Once);
        }

        [Fact]
        public void Delete_WhenTodoDoesNotExist_ShouldNotThrow()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Delete(999));

            // Act
            var exception = Record.Exception(() => _service.Delete(999));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Delete_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Delete(1))
                .Throws(new InvalidOperationException("Database error"));

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => _service.Delete(1));

            // Assert
            Assert.Equal("Database error", exception.Message);
        }
        #endregion
    }
}
