using TaskApi.Models;
using TaskApi.Repositories;
using TaskStatus = TaskApi.Models.TaskStatus;

namespace TaskApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        // Construtor que recebe o repositório via injeção de dependência
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // Retorna todas as tarefas
        public List<TaskItem> GetAll() => _taskRepository.GetAll();

        // Retorna uma tarefa pelo ID
        public TaskItem? GetById(int id) => _taskRepository.GetById(id);

        // Cria uma nova tarefa
        public TaskItem Create(TaskItem newTask)
        {
            return _taskRepository.Create(newTask);
        }

        // Atualiza uma tarefa existente
        public TaskItem Update(TaskItem updatedTask)
        {
            return _taskRepository.Update(updatedTask);
        }

        // Atualiza apenas o status de uma tarefa
        public void UpdateStatus(int id, TaskStatus status)
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