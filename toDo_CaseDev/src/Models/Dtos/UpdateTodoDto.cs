using System.ComponentModel.DataAnnotations;
using TodoApi.Models;
public class UpdateTodoDto
{
    [EnumDataType(typeof(TodoStatus), ErrorMessage = "O valor do status é inválido.")]
    public TodoStatus Status { get; set; }
}