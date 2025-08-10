using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        List<TodoItem> GetAll();
        TodoItem? GetById(int id);
        TodoItem Create(TodoItem newTask);
        TodoItem Update(TodoItem updatedTask);
        void UpdateStatus(int id, TodoStatus newStatus);
        void Delete(int id);
    }
}
