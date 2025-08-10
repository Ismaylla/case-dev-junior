using TodoApi.Models;

namespace TodoApi.Repositories {
    public class TaskRepository : ITaskRepository
    {
        private readonly Dictionary<int, TodoItem> _tasks;
        private int _nextId;

        // Construtor que inicializa o repositório com um dicionário vazio
        public TaskRepository()
        {
            _tasks = new Dictionary<int, TodoItem>();
            _nextId = _tasks.Any() ? _tasks.Keys.Max() + 1 : 1;
        }

        // Retorna todas as tarefas
        public List<TodoItem> GetAll()
        {
            return _tasks.Values.ToList();
        }

        // Retorna uma tarefa pelo ID
        public TodoItem? GetById(int id) => _tasks.GetValueOrDefault(id);

        // Cria uma nova tarefa
        public TodoItem Create(TodoItem newTask)
        {
            if (string.IsNullOrWhiteSpace(newTask.Title))
            {
                throw new ArgumentException("O título da tarefa é obrigatório.", nameof(newTask));
            }

            newTask.Id = _nextId++;
            newTask.Status = TodoStatus.Pendente; // Define o status inicial como Pendente
            newTask.Description ??= string.Empty; // Garante que a descrição nunca seja nula
            _tasks[newTask.Id] = newTask;  // Adiciona ou substitui a tarefa
            return newTask;
        }

        // Atualiza uma tarefa existente
        public TodoItem Update(TodoItem updatedTask)
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
        public void UpdateStatus(int id, TodoStatus newStatus)
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