using System.ComponentModel.DataAnnotations;
using TaskStatus = TaskApi.Models.TaskStatus;

public class TaskDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O título da tarefa é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve ter entre 3 e 100 caracteres.")]
    public required string Title { get; set; }

    [StringLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "O status da tarefa é obrigatório.")]
    public required TaskStatus Status { get; set; }
}
