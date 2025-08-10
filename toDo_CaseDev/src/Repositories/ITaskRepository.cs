using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITaskRepository
    {
        List<TodoItem> GetAll();
        TodoItem? GetById(int id);
        TodoItem Create(TodoItem newTask);
        TodoItem Update(TodoItem updatedTask);
        void UpdateStatus(int id, TodoStatus newStatus);
        void Delete(int id);
    }
}