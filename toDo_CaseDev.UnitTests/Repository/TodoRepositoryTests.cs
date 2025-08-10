using TodoApi.Models;
using TodoApi.Repositories;

namespace toDo_CaseDev.UnitTests.Repository {
    public class TodoRepositoryTests
    {
        private readonly TaskRepository _repository;

        public TodoRepositoryTests()
        {
            _repository = new TaskRepository();
        }

        [Fact]
        public void GetAll_WhenEmpty_ShouldReturnEmptyList()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_WithTasks_ShouldReturnAllTasksAsDtos()
        {
            // Arrange
            var task1 = new TodoItem { Title = "Teste 1", Description = "Descrição 1" };
            var task2 = new TodoItem { Title = "Teste 2", Description = "Descrição 2" };

            _repository.Create(task1);
            _repository.Create(task2);

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Collection(result,
                dto =>
                {
                    Assert.Equal(task1.Title, dto.Title);
                    Assert.Equal(task1.Description, dto.Description);
                    Assert.Equal(TodoStatus.Pendente.ToString(), dto.Status);
                },
                dto =>
                {
                    Assert.Equal(task2.Title, dto.Title);
                    Assert.Equal(task2.Description, dto.Description);
                    Assert.Equal(TodoStatus.Pendente.ToString(), dto.Status);
                }
            );
        }

        [Fact]
        public void GetById_WhenTaskExists_ShouldReturnTask()
        {
            //Arrange
            var task = new TodoItem { Title = "Teste", Description = "Descrição" };
            var createdTask = _repository.Create(task);

            // Act
            var result = _repository.GetById(createdTask.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(task.Title, result.Title);
            Assert.Equal(task.Description, result.Description);
            Assert.Equal(TodoStatus.Pendente, result.Status);
        }

        [Fact]
        public void GetById_WhenTaskDoesNotExist_ShouldReturnNull()
        {
            // Act
            var result = _repository.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Create_ShouldAssignIdAndSetPendingStatus()
        {
            // Arrange
            var task = new TodoItem { Title = "Teste", Description = "Descrição" };

            // Act
            var result = _repository.Create(task);

            // Assert
            Assert.True(result.Id > 0);
            Assert.Equal(TodoStatus.Pendente, result.Status);
            Assert.Equal(task.Title, result.Title);
            Assert.Equal(task.Description, result.Description);
        }

        [Fact]
        public void Create_MultipleTasks_ShouldAssignUniqueIds()
        {
            // Arrange
            var task1 = new TodoItem { Title = "Teste 1", Description = "Descrição 1" };
            var task2 = new TodoItem { Title = "Teste 2", Description = "Descrição 2" };

            // Act
            var result1 = _repository.Create(task1);
            var result2 = _repository.Create(task2);

            // Assert
            Assert.NotEqual(result1.Id, result2.Id);
        }

        [Fact]
        public void UpdateStatus_WhenTaskExists_ShouldUpdateStatus()
        {
            // Arrange
            var task = new TodoItem { Title = "Teste", Description = "Descrição" };
            var createdTask = _repository.Create(task);

            // Act
            _repository.UpdateStatus(createdTask.Id, TodoStatus.EmAndamento);

            // Assert
            var updatedTask = _repository.GetById(createdTask.Id);
            Assert.NotNull(updatedTask);
            Assert.Equal(TodoStatus.EmAndamento, updatedTask.Status);
        }

        [Fact]
        public void UpdateStatus_WhenTaskDoesNotExist_ShouldNotThrowException()
        {
            // Act 
            var exception = Record.Exception(() => _repository.UpdateStatus(999, TodoStatus.EmAndamento));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public void Delete_WhenTaskExists_ShouldRemoveTask()
        {
            // Arrange
            var task = new TodoItem { Title = "Teste", Description = "Descrição" };
            var createdTask = _repository.Create(task);

            // Act
            _repository.Delete(createdTask.Id);

            // Assert
            var result = _repository.GetById(createdTask.Id);
            Assert.Null(result);
        }

        [Fact]
        public void Delete_WhenTaskDoesNotExist_ShouldNotThrowException()
        {
            // Arrange 

            //Act
            var exception = Record.Exception(() => _repository.Delete(999));

            //Assert 
            Assert.Null(exception); 
        }
    }
}
