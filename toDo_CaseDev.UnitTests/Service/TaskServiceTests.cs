using Moq;
using TaskApi.Models;
using TaskApi.Repositories;
using TaskApi.Services;
using TaskStatus = TaskApi.Models.TaskStatus;

namespace TaskApi.UnitTests.Service
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _service = new TaskService(_mockRepository.Object);
        }

        #region GetAll Tests
        [Fact]
        public void GetAll_ShouldReturnAllTasks()
        {
            // Arrange
            var expectedTasks = new List<TaskItem>
            {
                new() { Id = 1, Title = "Tarefa 1", Description = "Descrição 1", Status = TaskStatus.Pendente },
                new() { Id = 2, Title = "Tarefa 2", Description = "Descrição 2", Status = TaskStatus.EmAndamento }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(expectedTasks);

            // Act
            var result = _service.GetAll();

            // Assert
            Assert.Equal(expectedTasks, result);
            _mockRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public void GetAll_WhenEmpty_ShouldReturnEmptyList()
        {
            // Arrange
            var expectedTasks = new List<TaskItem>();
            _mockRepository.Setup(repo => repo.GetAll()).Returns(expectedTasks);

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
                .Throws(new InvalidOperationException("Erro de Dados"));

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => _service.GetAll());

            //Assert
            Assert.Equal("Erro de Dados", exception.Message);
        }
        #endregion

        #region GetById Tests
        [Fact]
        public void GetById_WhenTaskExists_ShouldReturnTask()
        {
            // Arrange
            var expectedTask = new TaskItem
            {
                Id = 1,
                Title = "Tarefa Teste",
                Description = "Descrição Teste",
                Status = TaskStatus.Pendente
            };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(expectedTask);

            // Act
            var result = _service.GetById(1);

            // Assert
            Assert.Equal(expectedTask, result);
            _mockRepository.Verify(repo => repo.GetById(1), Times.Once);
        }

        [Fact]
        public void GetById_WhenTaskDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(1)).Returns((TaskItem?)null);

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
                .Throws(new InvalidOperationException("Erro de Dados"));

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => _service.GetById(1));

            //Assert
            Assert.Equal("Erro de Dados", exception.Message);
        }
        #endregion

        #region Create Tests
        [Fact]
        public void Create_WithValidTask_ShouldCreateAndReturnTask()
        {
            // Arrange
            var newTask = new TaskItem
            {
                Title = "Nova Tarefa",
                Description = "Nova Descrição"
            };
            var createdTask = new TaskItem
            {
                Id = 1,
                Title = "Nova Tarefa",
                Description = "Nova Descrição",
                Status = TaskStatus.Pendente
            };
            _mockRepository.Setup(repo => repo.Create(It.IsAny<TaskItem>())).Returns(createdTask);

            // Act
            var result = _service.Create(newTask);

            // Assert
            Assert.Equal(createdTask, result);
            _mockRepository.Verify(repo => repo.Create(It.Is<TaskItem>(t =>
                t.Title == newTask.Title &&
                t.Description == newTask.Description)), Times.Once);
        }

        [Fact]
        public void Create_WithNullDescription_ShouldCreateWithEmptyDescription()
        {
            // Arrange
            var newTask = new TaskItem
            {
                Title = "Nova Tarefa",
                Description = null
            };
            var createdTask = new TaskItem
            {
                Id = 1,
                Title = "Nova Tarefa",
                Description = string.Empty,
                Status = TaskStatus.Pendente
            };
            _mockRepository.Setup(repo => repo.Create(It.IsAny<TaskItem>())).Returns(createdTask);

            // Act
            var result = _service.Create(newTask);

            // Assert
            Assert.Equal(string.Empty, result.Description);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithInvalidTitle_ShouldThrowArgumentException(string title)
        {
            // Arrange
            var newTask = new TaskItem
            {
                Title = title,
                Description = "Nova Descrição"
            };

            _mockRepository.Setup(repo => repo.Create(It.IsAny<TaskItem>()))
                .Throws(new ArgumentException("O título da tarefa é obrigatório.", "newTask"));

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _service.Create(newTask));

            // Assert
            Assert.Contains("O título da tarefa é obrigatório.", exception.Message);
        }
        #endregion

        #region Update Tests
        [Fact]
        public void Update_WithValidTask_ShouldUpdateAndReturnTask()
        {
            // Arrange
            var taskToUpdate = new TaskItem
            {
                Id = 1,
                Title = "Tarefa Atualizada",
                Description = "Descrição Atualizada"
            };
            _mockRepository.Setup(repo => repo.Update(It.IsAny<TaskItem>())).Returns(taskToUpdate);

            // Act
            var result = _service.Update(taskToUpdate);

            // Assert
            Assert.Equal(taskToUpdate, result);
            _mockRepository.Verify(repo => repo.Update(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public void Update_WithInvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var nonExistentTask = new TaskItem
            {
                Id = 999,
                Title = "Não Existe",
                Description = "Tarefa Inexistente"
            };
            _mockRepository.Setup(repo => repo.Update(It.IsAny<TaskItem>()))
                .Throws(new KeyNotFoundException("Tarefa não encontrada."));

            // Act
            var exception = Assert.Throws<KeyNotFoundException>(() => _service.Update(nonExistentTask));

            // Assert
            Assert.Equal("Tarefa não encontrada.", exception.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Update_WithInvalidTitle_ShouldThrowArgumentException(string title)
        {
            // Arrange
            var taskToUpdate = new TaskItem
            {
                Id = 1,
                Title = title,
                Description = "Descrição Atualizada"
            };
            _mockRepository.Setup(repo => repo.Update(It.IsAny<TaskItem>()))
                .Throws(new ArgumentException("O título da tarefa é obrigatório.", "updatedTask"));

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _service.Update(taskToUpdate));

            // Assert
            Assert.Contains("O título da tarefa é obrigatório.", exception.Message);
        }

        [Fact]
        public void Update_WithNullDescription_ShouldUpdateWithEmptyDescription()
        {
            // Arrange
            var taskToUpdate = new TaskItem
            {
                Id = 1,
                Title = "Tarefa Atualizada",
                Description = null
            };
            var updatedTask = new TaskItem
            {
                Id = 1,
                Title = "Tarefa Atualizada",
                Description = string.Empty
            };
            _mockRepository.Setup(repo => repo.Update(It.IsAny<TaskItem>())).Returns(updatedTask);

            // Act
            var result = _service.Update(taskToUpdate);

            // Assert
            Assert.Equal(string.Empty, result.Description);
        }
        #endregion

        #region UpdateStatus Tests
        [Fact]
        public void UpdateStatus_WhenTaskExists_ShouldUpdateStatus()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.UpdateStatus(1, TaskStatus.EmAndamento));

            // Act
            _service.UpdateStatus(1, TaskStatus.EmAndamento);

            // Assert
            _mockRepository.Verify(repo => repo.UpdateStatus(1, TaskStatus.EmAndamento), Times.Once);
        }

        [Fact]
        public void UpdateStatus_WhenTaskDoesNotExist_ShouldNotThrow()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.UpdateStatus(999, TaskStatus.EmAndamento));

            // Act
            var exception = Record.Exception(() => _service.UpdateStatus(999, TaskStatus.EmAndamento));

            // Assert
            Assert.Null(exception);
            _mockRepository.Verify(repo => repo.UpdateStatus(999, TaskStatus.EmAndamento), Times.Once);
        }

        [Fact]
        public void UpdateStatus_WithSameStatus_ShouldUpdateSuccessfully()
        {
            // Arrange
            var existingTask = new TaskItem
            {
                Id = 1,
                Title = "Tarefa",
                Description = "Descrição",
                Status = TaskStatus.EmAndamento
            };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(existingTask);
            _mockRepository.Setup(repo => repo.UpdateStatus(1, TaskStatus.EmAndamento));

            // Act
            _service.UpdateStatus(1, TaskStatus.EmAndamento);

            // Assert 
            _mockRepository.Verify(repo => repo.UpdateStatus(1, TaskStatus.EmAndamento), Times.Once);
        }
        #endregion

        #region Delete Tests
        [Fact]
        public void Delete_WhenTaskExists_ShouldDeleteTask()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Delete(1));

            // Act
            _service.Delete(1);

            // Assert
            _mockRepository.Verify(repo => repo.Delete(1), Times.Once);
        }

        [Fact]
        public void Delete_WhenTaskDoesNotExist_ShouldNotThrow()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Delete(999));

            // Act
            var exception = Record.Exception(() => _service.Delete(999));

            // Assert
            Assert.Null(exception);
            _mockRepository.Verify(repo => repo.Delete(999), Times.Once);
        }

        [Fact]
        public void Delete_WhenRepositoryThrows_ShouldPropagateException()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.Delete(1))
                .Throws(new InvalidOperationException("Erro de Dados"));

            // Act
            var exception = Assert.Throws<InvalidOperationException>(() => _service.Delete(1));

            // Assert
            Assert.Equal("Erro de Dados", exception.Message);
        }
        #endregion
    }
}
