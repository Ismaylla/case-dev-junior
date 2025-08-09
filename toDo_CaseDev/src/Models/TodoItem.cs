namespace TodoApi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public TodoStatus Status { get; set; } = TodoStatus.Pendente;
    }
}