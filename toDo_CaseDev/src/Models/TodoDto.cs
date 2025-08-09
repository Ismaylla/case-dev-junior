using TodoApi.Models;

public class TodoItemDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Status { get; set; } 
    
}