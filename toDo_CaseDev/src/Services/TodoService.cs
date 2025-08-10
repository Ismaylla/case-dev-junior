using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services {
    public class TodoService : ITodoService
    {
        private readonly ITaskRepository _taskRepository;

        // Construtor que recebe o repositório via injeção de dependência
        public TodoService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // Retorna todas as tarefas
        public List<TodoItem> GetAll() => _taskRepository.GetAll();

        // Retorna uma tarefa pelo ID
        public TodoItem? GetById(int id) => _taskRepository.GetById(id);

        // Cria uma nova tarefa
        public TodoItem Create(TodoItem newTask)
        {
            return _taskRepository.Create(newTask);
        }

        // Atualiza uma tarefa existente
        public TodoItem Update(TodoItem updatedTask)
        {
            return _taskRepository.Update(updatedTask);
        }

        // Atualiza apenas o status de uma tarefa
        public void UpdateStatus(int id, TodoStatus status)
        {
            _taskRepository.UpdateStatus(id, status);
        }

        // Remove uma tarefa pelo ID
        public void Delete(int id)
        {
            _taskRepository.Delete(id);
        }

        
    }
}