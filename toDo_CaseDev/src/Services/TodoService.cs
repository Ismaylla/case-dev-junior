using System.Text.Json;
using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoService
    {
        private const string JsonFilePath = "tasks.json";
        private readonly List<TodoItem> _tasks;
        private int _nextId = 1;

        public TodoService()
        {
            _tasks = LoadTasksFromFile();

            if (_tasks.Any())
            {
                _nextId = _tasks.Max(t => t.Id) + 1;
            }
        }

        private List<TodoItem> LoadTasksFromFile()
        {
            if (!File.Exists(JsonFilePath))
            {
                return new List<TodoItem>();
            }

            var json = File.ReadAllText(JsonFilePath);

            if (string.IsNullOrEmpty(json))
            {
                return new List<TodoItem>();
            }

            return JsonSerializer.Deserialize<List<TodoItem>>(json) ?? new List<TodoItem>();
        }

        private void SaveTasksToFile()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(_tasks, options);
            File.WriteAllText(JsonFilePath, json);
        }

        public List<TodoItem> GetAll() => _tasks;

        public TodoItem? GetById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

        public TodoItem Create(TodoItem newTask)
        {
            newTask.Id = _nextId++;
            newTask.Status = TodoStatus.Pendente;
            _tasks.Add(newTask);
            SaveTasksToFile();
            return newTask;
        }

        public void UpdateStatus(int id, TodoStatus newStatus)
        {
            var taskToUpdate = GetById(id);
            if (taskToUpdate != null)
            {
                taskToUpdate.Status = newStatus;
                SaveTasksToFile();
            }
        }

        public void Delete(int id)
        {
            var taskToDelete = GetById(id);
            if (taskToDelete != null)
            {
                _tasks.Remove(taskToDelete);
                SaveTasksToFile();
            }
        }
    }
}