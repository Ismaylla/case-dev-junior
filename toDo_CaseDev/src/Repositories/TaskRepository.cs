using TodoApi.Models;

namespace TodoApi.Repositories {
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
            if (string.IsNullOrWhiteSpace(newTask.Title))
            {
                throw new ArgumentException("O título da tarefa é obrigatório.", nameof(newTask));
            }

            newTask.Id = _nextId++;
            newTask.Status = TodoStatus.Pendente;
            newTask.Description ??= string.Empty; // Ensure description is never null
            _tasks[newTask.Id] = newTask;  // Adiciona ou substitui a tarefa
            return newTask;
        }

        public TodoItem Update(TodoItem updatedTask)
        {
            if (string.IsNullOrWhiteSpace(updatedTask.Title))
            {
                throw new ArgumentException("O título da tarefa é obrigatório.", nameof(updatedTask));
            }

            if (_tasks.ContainsKey(updatedTask.Id))
            {
                updatedTask.Description ??= string.Empty; // Ensure description is never null
                _tasks[updatedTask.Id] = updatedTask;
                return updatedTask;
            }
            throw new KeyNotFoundException("Tarefa não encontrada.");
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