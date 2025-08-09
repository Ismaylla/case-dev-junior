using TodoApi.Models;
using System.Text.Json;

namespace TodoApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private const string JsonFilePath = "tasks.json";
        private readonly Dictionary<int, TodoItem> _tasks;
        private int _nextId;

        public TaskRepository()
        {
            _tasks = LoadTasksFromFile();
            _nextId = _tasks.Any() ? _tasks.Keys.Max() + 1 : 1;
        }

        private Dictionary<int, TodoItem> LoadTasksFromFile()
        {
            if (!File.Exists(JsonFilePath)) return new Dictionary<int, TodoItem>();

            var json = File.ReadAllText(JsonFilePath);
            if (string.IsNullOrEmpty(json)) return new Dictionary<int, TodoItem>();

            var tasks = JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
            return tasks.ToDictionary(t => t.Id);
        }

        private void SaveTasksToFile()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_tasks.Values.ToList(), options); // Converte o Dictionary em uma lista
            File.WriteAllText(JsonFilePath, json);
        }

        public List<TodoItem> GetAll() => _tasks.Values.ToList();

        public TodoItem? GetById(int id) => _tasks.GetValueOrDefault(id);

        public TodoItem Create(TodoItem newTask)
        {
            newTask.Id = _nextId++;
            newTask.Status = TodoStatus.Pendente;
            _tasks[newTask.Id] = newTask;  // Adiciona ou substitui a tarefa
            SaveTasksToFile();
            return newTask;
        }

        public void UpdateStatus(int id, TodoStatus newStatus)
        {
            if (_tasks.TryGetValue(id, out var taskToUpdate))
            {
                taskToUpdate.Status = newStatus;
                SaveTasksToFile();
            }
        }
        
        public void Delete(int id)
        {
            if (_tasks.Remove(id))
            {
                SaveTasksToFile();
            }
        }

    }
}