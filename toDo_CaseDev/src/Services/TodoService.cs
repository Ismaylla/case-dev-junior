using System.Text.Json;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService
    {
        private readonly ITaskRepository _taskRepository;

        public TodoService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public List<TodoItem> GetAll() => _taskRepository.GetAll();
        public TodoItem? GetById(int id) => _taskRepository.GetById(id);

        public TodoItem Create(TodoItem newTask)
        {
            if (string.IsNullOrEmpty(newTask.Title))
            {
                throw new ArgumentException("O título é obrigatório.", nameof(newTask.Title));
            }
            return _taskRepository.Create(newTask);
        }

        public void UpdateStatus(int id, TodoStatus newStatus)
        {
            var task = _taskRepository.GetById(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
            }
            _taskRepository.UpdateStatus(id, newStatus);
        }

        public void Delete(int id)
        {
            var task = _taskRepository.GetById(id);
            if (task == null)
            {
                throw new KeyNotFoundException($"Tarefa com ID {id} não encontrada.");
            }
            _taskRepository.Delete(id);
        }

        
    }
}