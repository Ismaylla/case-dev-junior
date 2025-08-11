using TaskApi.Models;
using TaskStatus = TaskApi.Models.TaskStatus;

namespace TaskApi.Services
{
    public interface ITaskService
    {
        List<TaskItem> GetAll();
        TaskItem? GetById(int id);
        TaskItem Create(TaskItem newTask);
        TaskItem Update(TaskItem updatedTask);
        void UpdateStatus(int id, TaskStatus status);
        void Delete(int id);
    }
}
