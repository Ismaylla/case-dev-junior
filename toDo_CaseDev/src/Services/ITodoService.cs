using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        List<TodoItemDto> GetAll();
        TodoItem? GetById(int id);
        TodoItem Create(TodoItem newTask);
        void UpdateStatus(int id, TodoStatus newStatus);
        void Delete(int id);
    }
}
