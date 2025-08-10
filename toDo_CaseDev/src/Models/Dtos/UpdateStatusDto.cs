using System.ComponentModel.DataAnnotations;
using TodoApi.Models;
public class UpdateStatusTodoDto
{
    [EnumDataType(typeof(TodoStatus), ErrorMessage = "O valor do status deve estar entre 0 e 2.")]
    public TodoStatus Status { get; set; }
}