using Microsoft.AspNetCore.Mvc;
using TaskApi.Exceptions;
using TaskApi.Services;
using TaskApi.Models;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITaskService _todoService;
        private readonly ILogger<TodoController> _logger;

        public TodoController(ITaskService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/todo - Lista todas as tarefas.
        [HttpGet]
        public ActionResult<List<TaskDto>> GetAll()
        {
            try
            {
                var tasks = _todoService.GetAll();
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
                return BadRequest(new ApiErrorResponse("Erro Interno", ex.Message));
            }
        }

        // GET: api/todo/{id} - Obtém uma tarefa pelo ID.
        [HttpGet("{id}")]
        public ActionResult<TaskDto> GetById(int id)
        {
            try
            {
                var task = _todoService.GetById(id);
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
        public ActionResult<TaskItem> Create([FromBody] CreateTaskDto todoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var todoItem = new TaskItem
                {
                    Title = todoDto.Title,
                    Description = todoDto.Description ?? string.Empty
                };

                var newTodo = _todoService.Create(todoItem);

                return CreatedAtAction(nameof(GetById), new { id = newTodo.Id }, newTodo);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse("Erro ao criar tarefa", ex.Message));
            }
        }

        // PUT: api/todo/{id} - Atualiza uma tarefa.
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateTaskDto todoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var task = _todoService.GetById(id);

                if (task == null)
                    return NotFound(new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe."));

                task.Title = todoDto.Title;
                task.Description = todoDto.Description ?? string.Empty;

                var updatedTodo = _todoService.Update(task);
                return Ok(updatedTodo);
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
                var task = _todoService.GetById(id);

                if (task == null)
                    return NotFound(new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe."));

                _todoService.UpdateStatus(id, statusDto.Status);

                // Após atualizar o status, buscar a tarefa atualizada para resposta correta
                var updatedTask = _todoService.GetById(id);

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
                var task = _todoService.GetById(id);

                if (task == null)
                    return NotFound(new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe."));

                _todoService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse("Erro ao deletar tarefa", ex.Message));
            }
        }
    }
}
