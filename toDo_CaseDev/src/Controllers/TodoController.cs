using Microsoft.AspNetCore.Mvc;
using TodoApi.Exceptions;
using TodoApi.Services;
using TodoApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Requer autenticação para acessar os endpoints
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/todo - Lista todas as tarefas.
        [HttpGet]
        public ActionResult<List<TodoDto>> GetAll() // Corrigido para retornar o DTO de saída
        {
            var tasks = _todoService.GetAll();
            // Mapeia o modelo interno para o DTO de saída
            var tasksDto = tasks.Select(task => new TodoDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.GetDescription()
            }).ToList();

            return Ok(tasksDto);
        }

        // GET: api/todo/{id} - Obtém uma tarefa pelo ID.
        [HttpGet("{id}")]
        public ActionResult<TodoDto> GetById(int id)
        {
            var task = _todoService.GetById(id);
            if (task == null)
            {
                var errorResponse = new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe.");
                return NotFound(errorResponse);
            }

            var taskDto = new TodoDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status.GetDescription()
            };

            return Ok(taskDto);
        }

        // POST: api/todo - Cria uma nova tarefa.
        [HttpPost]
        public ActionResult<TodoItem> Create(CreateTodoDto todoDto)
        {
            if (string.IsNullOrWhiteSpace(todoDto.Title))
            {
                return BadRequest(new ApiErrorResponse("Erro de Validação", "O título é obrigatório."));
            }

            var todoItem = new TodoItem
            {
                Title = todoDto.Title,
                Description = todoDto.Description ?? string.Empty
            };

            var newTodo = _todoService.Create(todoItem);

            return CreatedAtAction(nameof(GetAll), new { id = newTodo.Id }, newTodo);
        }

        // PUT: api/todo/{id} - Atualiza uma tarefa.
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateTodoDto todoDto)
        {
            if (string.IsNullOrWhiteSpace(todoDto.Title))
            {
                return BadRequest(new ApiErrorResponse("Erro de Validação", "O título é obrigatório."));
            }

            var task = _todoService.GetById(id);

            if (task == null)
            {
                var errorResponse = new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe.");
                return NotFound(errorResponse);
            }

            task.Title = todoDto.Title;
            task.Description = todoDto.Description ?? string.Empty;

            var updatedTodo = _todoService.Update(task);
            return Ok(updatedTodo);
        }

        // PUT: api/todo/{id}/status - Atualiza o status de uma tarefa.
        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateStatusTodoDto statusDto)
        {
            var task = _todoService.GetById(id);

            if (task == null)
            {
                var errorResponse = new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe.");
                return NotFound(errorResponse);
            }

            _todoService.UpdateStatus(id, statusDto.Status);
            var taskDto = new TodoDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = statusDto.Status.GetDescription()
            };
            return Ok(taskDto);
        }

        // DELETE: api/todo/{id} - Deleta uma tarefa.
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = _todoService.GetById(id);

            if (task == null)
            {
                var errorResponse = new ApiErrorResponse("Recurso Não Encontrado", "A tarefa com o ID fornecido não existe.");
                return NotFound(errorResponse);
            }

            _todoService.Delete(id);
            return NoContent();
        }
    }
}