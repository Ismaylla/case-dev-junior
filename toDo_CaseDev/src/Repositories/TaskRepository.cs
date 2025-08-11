using TaskApi.Models;
using TaskStatus = TaskApi.Models.TaskStatus;

namespace TaskApi.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly Dictionary<int, TaskItem> _tasks;
        private int _nextId;

        // Construtor que inicializa o repositório com um dicionário vazio
        public TaskRepository()
        {
            _tasks = new Dictionary<int, TaskItem>();
            _nextId = _tasks.Any() ? _tasks.Keys.Max() + 1 : 1;
        }

        // Retorna todas as tarefas
        public List<TaskItem> GetAll()
        {
            return _tasks.Values.ToList();
        }

        // Retorna uma tarefa pelo ID
        public TaskItem? GetById(int id) => _tasks.GetValueOrDefault(id);

        // Cria uma nova tarefa
        public TaskItem Create(TaskItem newTask)
        {
            if (string.IsNullOrWhiteSpace(newTask.Title))
            {
                throw new ArgumentException("O título da tarefa é obrigatório.", nameof(newTask));
            }

            newTask.Id = _nextId++;
            newTask.Status = TaskStatus.Pendente; // Define o status inicial como Pendente
            newTask.Description ??= string.Empty; // Garante que a descrição nunca seja nula
            _tasks[newTask.Id] = newTask;  // Adiciona ou substitui a tarefa
            return newTask;
        }

        // Atualiza uma tarefa existente
        public TaskItem Update(TaskItem updatedTask)
        {
            if (string.IsNullOrWhiteSpace(updatedTask.Title))
            {
                throw new ArgumentException("O título da tarefa é obrigatório.", nameof(updatedTask));
            }

            if (_tasks.ContainsKey(updatedTask.Id))
            {
                updatedTask.Description ??= string.Empty; // Garante que a descrição nunca seja nula
                _tasks[updatedTask.Id] = updatedTask;
                return updatedTask;
            }
            throw new KeyNotFoundException("Tarefa não encontrada.");
        }

        // Atualiza apenas o status de uma tarefa
        public void UpdateStatus(int id, TaskStatus newStatus)
        {
            if (_tasks.TryGetValue(id, out var taskToUpdate))
            {
                taskToUpdate.Status = newStatus;
            }
        }

        // Remove uma tarefa pelo ID
        public void Delete(int id)
        {
            if (_tasks.ContainsKey(id))
            {
                _tasks.Remove(id);
            }
        }

    }
}