using TodoApi.Models;
using System.Text.Json;

namespace TodoApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly Dictionary<int, TodoItem> _tasks;
        private int _nextId;

        public TaskRepository()
        {
            _tasks = new Dictionary<int,TodoItem>();
            _nextId = _tasks.Any() ? _tasks.Keys.Max() + 1 : 1;
        }

        public List<TodoItem> GetAll()
        {
            return _tasks.Values.ToList();
        }

        public TodoItem? GetById(int id) => _tasks.GetValueOrDefault(id);

        public TodoItem Create(TodoItem newTask)
        {
            newTask.Id = _nextId++;
            newTask.Status = TodoStatus.Pendente;
            _tasks[newTask.Id] = newTask;  // Adiciona ou substitui a tarefa
            return newTask;
        }

        public void UpdateStatus(int id, TodoStatus newStatus)
        {
            if (_tasks.TryGetValue(id, out var taskToUpdate))
            {
                taskToUpdate.Status = newStatus;
            }
        }
        
        public void Delete(int id)
        {
            if (_tasks.ContainsKey(id))
            {
                _tasks.Remove(id);
            }
        }

    }
}