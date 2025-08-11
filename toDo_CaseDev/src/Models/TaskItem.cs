namespace TaskApi.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; } //Descrição pode ser opcional

        public TaskStatus Status { get; set; } = TaskStatus.Pendente;
    }
}
