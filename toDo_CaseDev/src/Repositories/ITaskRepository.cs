using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITaskRepository
    {
        List<TodoItemDto> GetAll();
        TodoItem? GetById(int id);
        TodoItem Create(TodoItem newTask);
        void UpdateStatus(int id, TodoStatus newStatus);
        void Delete(int id);
    }
}