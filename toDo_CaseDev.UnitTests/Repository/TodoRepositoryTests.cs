using TodoApi.Models;
using TodoApi.Repositories;

namespace toDo_CaseDev.UnitTests.Repository {
    public class TodoRepositoryTests
    {
        private readonly TaskRepository _repository;

        public TodoRepositoryTests()
        {
            _repository = new TaskRepository();
            // Clear any existing tasks before each test
            foreach (var task in _repository.GetAll().ToList())
            {
                _repository.Delete(task.Id);
            }
        }

        #region GetAll Tests
        [Fact]
        public void GetAll_WhenEmpty_ReturnsEmptyList()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetAll_WithTasks_ReturnsAllTasks()
        {
            // Arrange
            var task1 = new TodoItem { Title = "Título 1", Description = "Descrição 1" };
            var task2 = new TodoItem { Title = "Título 2", Description = "Descrição 2" };

            _repository.Create(task1);
            _repository.Create(task2);

            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Title == "Título 1" && t.Description == "Descrição 1");
            Assert.Contains(result, t => t.Title == "Título 2" && t.Description == "Descrição 2");
        }

        [Fact]
        public void GetAll_AfterDelete_ReturnsRemainingTasks()
        {
            // Arrange
            var task1 = _repository.Create(new TodoItem { Title = "Tarefa 1", Description = "Descrição 1" });
            var task2 = _repository.Create(new TodoItem { Title = "Tarefa 2", Description = "Descrição 2" });

            // Act
            _repository.Delete(task1.Id);
            var result = _repository.GetAll();

            // Assert
            Assert.Single(result);
            Assert.Equal(task2.Id, result[0].Id);
        }
        #endregion

        #region GetById Tests
        [Fact]
        public void GetById_WhenTaskExists_ReturnsTodoItem()
        {
            // Arrange
            var task = new TodoItem { Title = "Título da task", Description = "Descrição da task" };
            var createdTask = _repository.Create(task);

            // Act
            var result = _repository.GetById(createdTask.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Título da task", result.Title);
            Assert.Equal("Descrição da task", result.Description);
            Assert.Equal(TodoStatus.Pendente, result.Status);
        }

        [Fact]
        public void GetById_WhenTaskDoesNotExist_ReturnsNull()
        {
            // Act
            var result = _repository.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetById_AfterUpdate_ReturnsUpdatedTask()
        {
            // Arrange
            var task = _repository.Create(new TodoItem { Title = "Original", Description = "Original" });
            var updated = new TodoItem 
            { 
                Id = task.Id, 
                Title = "Updated", 
                Description = "Updated", 
                Status = TodoStatus.EmAndamento 
            };
            _repository.Update(updated);

            // Act
            var result = _repository.GetById(task.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated", result.Title);
            Assert.Equal("Updated", result.Description);
            Assert.Equal(TodoStatus.EmAndamento, result.Status);
        }
        #endregion

        #region Create Tests
        [Fact]
        public void Create_WithEmptyTitle_ThrowsArgumentException()
        {
            // Arrange
            var task = new TodoItem { Title = string.Empty, Description = "Descrição" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _repository.Create(task));
        }

        [Fact]
        public void Create_WithNullTitle_ThrowsArgumentException()
        {
            // Arrange
            var task = new TodoItem { Title = null!, Description = "Descrição" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _repository.Create(task));
        }

        [Fact]
        public void Create_WithWhitespaceTitle_ThrowsArgumentException()
        {
            // Arrange
            var task = new TodoItem { Title = "   ", Description = "Descrição" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _repository.Create(task));
        }

        [Fact]
        public void Create_AssignsIdAndSetsPendingStatus()
        {
            // Arrange
            var task = new TodoItem { Title = "Título da task", Description = "Descrição da task" };

            // Act
            var result = _repository.Create(task);

            // Assert
            Assert.True(result.Id > 0);
            Assert.Equal(TodoStatus.Pendente, result.Status);
            Assert.Equal("Título da task", result.Title);
            Assert.Equal("Descrição da task", result.Description);
        }

        [Fact]
        public void Create_MultipleTasks_AssignsUniqueIds()
        {
            // Arrange
            var task1 = new TodoItem { Title = "Tarefa 1", Description = "Primeira" };
            var task2 = new TodoItem { Title = "Tarefa 2", Description = "Segunda" };

            // Act
            var result1 = _repository.Create(task1);
            var result2 = _repository.Create(task2);

            // Assert
            Assert.NotEqual(result1.Id, result2.Id);
            Assert.True(result2.Id > result1.Id);
        }

        [Fact]
        public void Create_WithNullDescription_SetsEmptyString()
        {
            // Arrange
            var task = new TodoItem { Title = "Tarefa", Description = null! };

            // Act
            var result = _repository.Create(task);

            // Assert
            Assert.Equal(string.Empty, result.Description);
        }

        [Fact]
        public void Create_PreservesInputData()
        {
            // Arrange
            var task = new TodoItem 
            { 
                Title = "Título da task",
                Description = "Descrição da task",
                Status = TodoStatus.EmAndamento
            };

            // Act
            var result = _repository.Create(task);

            // Assert
            Assert.Equal("Título da task", result.Title);
            Assert.Equal("Descrição da task", result.Description);
            Assert.Equal(TodoStatus.Pendente, result.Status);
        }
        #endregion

        #region Update Tests
        [Fact]
        public void Update_WithEmptyTitle_ThrowsArgumentException()
        {
            // Arrange
            var task = _repository.Create(new TodoItem { Title = "Título da task", Description = "Descrição da task" });
            var updateTask = new TodoItem { Id = task.Id, Title = string.Empty, Description = "Desc" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _repository.Update(updateTask));
        }

        [Fact]
        public void Update_WithNullTitle_ThrowsArgumentException()
        {
            // Arrange
            var task = _repository.Create(new TodoItem { Title = "Título da task", Description = "Descrição da task" });
            var updateTask = new TodoItem { Id = task.Id, Title = null!, Description = "Descrição" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _repository.Update(updateTask));
        }

        [Fact]
        public void Update_WithWhitespaceTitle_ThrowsArgumentException()
        {
            // Arrange
            var task = _repository.Create(new TodoItem { Title = "Título da task", Description = "Descrição da task" });
            var updateTask = new TodoItem { Id = task.Id, Title = "   ", Description = "Descrição" };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _repository.Update(updateTask));
        }

        [Fact]
        public void Update_WhenTaskExists_UpdatesAndReturnsTodoItem()
        {
            // Arrange
            var task = new TodoItem { Title = "Título da task", Description = "Título da task" };
            var created = _repository.Create(task);

            var updated = new TodoItem
            {
                Id = created.Id,
                Title = "Atualizada",
                Description = "Descrição Atualizada",
                Status = TodoStatus.EmAndamento
            };

            // Act
            var result = _repository.Update(updated);

            // Assert
            Assert.Equal(updated.Title, result.Title);
            Assert.Equal(updated.Description, result.Description);
            Assert.Equal(updated.Status, result.Status);
        }

        [Fact]
        public void Update_WhenTaskDoesNotExist_ThrowsKeyNotFoundException()
        {
            // Arrange
            var nonExistentTask = new TodoItem
            {
                Id = 999,
                Title = "Não Existe",
                Description = "Inexistente"
            };

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() => _repository.Update(nonExistentTask));
        }

        [Fact]
        public void Update_PreservesId()
        {
            // Arrange
            var original = _repository.Create(new TodoItem { Title = "Título da task", Description = "Descrição da task" });
            var updated = new TodoItem
            {
                Id = original.Id,
                Title = "Updated",
                Description = "Updated",
                Status = TodoStatus.EmAndamento
            };

            // Act
            var result = _repository.Update(updated);

            // Assert
            Assert.Equal(original.Id, result.Id);
        }
        #endregion

        #region UpdateStatus Tests
        [Fact]
        public void UpdateStatus_WhenTaskExists_UpdatesStatus()
        {
            // Arrange
            var task = new TodoItem { Title = "Pendente", Description = "Para Atualizar" };
            var created = _repository.Create(task);

            // Act
            _repository.UpdateStatus(created.Id, TodoStatus.EmAndamento);

            // Assert
            var updated = _repository.GetById(created.Id);
            Assert.NotNull(updated);
            Assert.Equal(TodoStatus.EmAndamento, updated.Status);
            Assert.Equal("Pendente", updated.Title); // outros campos permanecem iguais
        }

        [Fact]
        public void UpdateStatus_WhenTaskDoesNotExist_DoesNotThrow()
        {
            // Act
            var exception = Record.Exception(() => _repository.UpdateStatus(999, TodoStatus.EmAndamento));

            //Assert
            Assert.Null(exception);
        }

        [Fact]
        public void UpdateStatus_OnlyChangesStatus()
        {
            // Arrange
            var task = _repository.Create(new TodoItem 
            { 
                Title = "Título da task", 
                Description = "Descrição" 
            });

            // Act
            _repository.UpdateStatus(task.Id, TodoStatus.EmAndamento);

            // Assert
            var updated = _repository.GetById(task.Id);
            Assert.NotNull(updated);
            Assert.Equal("Título da task", updated.Title);
            Assert.Equal("Descrição", updated.Description);
            Assert.Equal(TodoStatus.EmAndamento, updated.Status);
        }

        [Fact]
        public void UpdateStatus_CanChangeMultipleTimes()
        {
            // Arrange
            var task = _repository.Create(new TodoItem { Title = "Test", Description = "Test" });

            // Act
            _repository.UpdateStatus(task.Id, TodoStatus.EmAndamento);

            // Assert
            var inProgress = _repository.GetById(task.Id);
            Assert.Equal(TodoStatus.EmAndamento, inProgress?.Status);

            //Act
            _repository.UpdateStatus(task.Id, TodoStatus.Concluida);

            //Assert
            var completed = _repository.GetById(task.Id);
            Assert.Equal(TodoStatus.Concluida, completed?.Status);
        }
        #endregion

        #region Delete Tests
        [Fact]
        public void Delete_WhenTaskExists_RemovesTask()
        {
            // Arrange
            var task = new TodoItem { Title = "Task para deletar", Description = "Será Removida" };
            var created = _repository.Create(task);

            // Act
            _repository.Delete(created.Id);

            // Assert
            var result = _repository.GetById(created.Id);
            Assert.Null(result);
            Assert.DoesNotContain(_repository.GetAll(), t => t.Id == created.Id);
        }

        [Fact]
        public void Delete_WhenTaskDoesNotExist_DoesNothing()
        {
            // Arrange
            var initialCount = _repository.GetAll().Count;

            // Act
            _repository.Delete(999);

            // Assert
            Assert.Equal(initialCount, _repository.GetAll().Count);
        }

        [Fact]
        public void Delete_RemovesOnlySpecifiedTask()
        {
            // Arrange
            var task1 = _repository.Create(new TodoItem { Title = "Task 1", Description = "Descrição 1" });
            var task2 = _repository.Create(new TodoItem { Title = "Task 2", Description = "Descrição 2" });

            // Act
            _repository.Delete(task1.Id);

            // Assert
            Assert.Null(_repository.GetById(task1.Id));
            Assert.NotNull(_repository.GetById(task2.Id));
        }
        #endregion

        #region Integration Tests
        [Fact]
        public void FullLifecycle_CreateUpdateStatusDelete_WorksAsExpected()
        {
            // Create
            var newTask = new TodoItem { Title = "Teste geral", Description = "Descrição do teste geral" };
            var created = _repository.Create(newTask);
            Assert.NotNull(created);
            Assert.Equal(TodoStatus.Pendente, created.Status);

            // Update Status
            _repository.UpdateStatus(created.Id, TodoStatus.EmAndamento);
            var inProgress = _repository.GetById(created.Id);
            Assert.NotNull(inProgress);
            Assert.Equal(TodoStatus.EmAndamento, inProgress.Status);

            // Update Content
            var updated = new TodoItem
            {
                Id = created.Id,
                Title = "Título atualizado",
                Description = "Descrição atualizada",
                Status = TodoStatus.Concluida
            };
            var updatedResult = _repository.Update(updated);
            Assert.Equal("Título atualizado", updatedResult.Title);
            Assert.Equal(TodoStatus.Concluida, updatedResult.Status);

            // Delete
            _repository.Delete(created.Id);
            Assert.Null(_repository.GetById(created.Id));
        }
        #endregion
    }
}
