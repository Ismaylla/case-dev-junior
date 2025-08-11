using System.ComponentModel.DataAnnotations;
using TaskApi.Models;
using TaskStatus = TaskApi.Models.TaskStatus;

public class UpdateTaskStatusDto
{
    [EnumDataType(typeof(TaskStatus), ErrorMessage = "O valor do status deve estar entre 0 e 2.")]
    public TaskStatus Status { get; set; }
}
