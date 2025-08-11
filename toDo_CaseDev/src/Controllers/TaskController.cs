using Microsoft.AspNetCore.Mvc;
using TaskApi.Exceptions;
using TaskApi.Services;
using TaskApi.Models;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        // GET: api/todo - Lista todas as tarefas.
        [HttpGet]
        public ActionResult<List<TaskDto>> GetAll()
        {

            _logger.LogInformation("Iniciando busca de todos os registros de task, para usuário.");

            try
            {
                var tasks = _taskService.GetAll();
                var tasksDto = tasks.Select(task => new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status
                }).ToList();

                return Ok(tasksDto);
            }
            catch (Exception ex)
            {

                _logger.LogInformation(ex.Message);
                return BadRequest(new ApiErrorResponse("Erro Interno", "asfasasasas"));

            }
        }

        // GET: api/todo/{id} - Obtém uma tarefa pelo ID.
        [HttpGet("{id}")]
        public ActionResult<TaskDto> GetById(int id)
        {
            try
            {
                var task = _taskService.GetById(id);
                if (task == null)
                    return NotFound(new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe."));

                var taskDto = new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status
                };

                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse("Erro Interno", ex.Message));
            }
        }

        // POST: api/todo - Cria uma nova tarefa.
        [HttpPost]
        public ActionResult<TaskItem> Create([FromBody] CreateTaskDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var taskItem = new TaskItem
                {
                    Title = taskDto.Title,
                    Description = taskDto.Description ?? string.Empty
                };

                var newTask = _taskService.Create(taskItem);

                return CreatedAtAction(nameof(GetById), new { id = newTask.Id }, newTask);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse("Erro ao criar tarefa", ex.Message));
            }
        }

        // PUT: api/todo/{id} - Atualiza uma tarefa.
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateTaskDto taskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var task = _taskService.GetById(id);

                if (task == null)
                    return NotFound(new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe."));

                task.Title = taskDto.Title;
                task.Description = taskDto.Description ?? string.Empty;

                var updatedTask = _taskService.Update(task);
                return Ok(updatedTask);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse("Erro ao atualizar tarefa", ex.Message));
            }
        }

        // PUT: api/todo/{id}/status - Atualiza o status de uma tarefa.
        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateTaskStatusDto statusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var task = _taskService.GetById(id);

                if (task == null)
                    return NotFound(new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe."));

                _taskService.UpdateStatus(id, statusDto.Status);

                // Após atualizar o status, buscar a tarefa atualizada para resposta correta
                var updatedTask = _taskService.GetById(id);

                var taskDto = new TaskDto
                {
                    Id = updatedTask!.Id,
                    Title = updatedTask.Title,
                    Description = updatedTask.Description,
                    Status = updatedTask.Status
                };

                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse("Erro ao atualizar status", ex.Message));
            }
        }

        // DELETE: api/todo/{id} - Deleta uma tarefa.
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var task = _taskService.GetById(id);

                if (task == null)
                    return NotFound(new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe."));

                _taskService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse("Erro ao deletar tarefa", ex.Message));
            }
        }
    }
}
