using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/todo - Lista todas as tarefas.
        [HttpGet]
        public ActionResult<List<TodoItemDto>> GetAll()
        {
            return _todoService.GetAll();
        }

        // POST: api/todo - Cria uma nova tarefa.
        [HttpPost]
        public ActionResult<TodoItem> Create(TodoItem todo)
        {
            if (string.IsNullOrEmpty(todo.Title))
            {
                return BadRequest("O título é obrigatório.");
            }

            var newTodo = _todoService.Create(todo);
            return CreatedAtAction(nameof(GetAll), new { id = newTodo.Id }, newTodo);
        }

        // PUT: api/todo/{id}/status - Atualiza o status de uma tarefa.
        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] TodoStatus newStatus)
        {
            var task = _todoService.GetById(id);

            if (task == null)
            {
                return NotFound();
            }

            _todoService.UpdateStatus(id, newStatus);
            return NoContent();
        }

        // DELETE: api/todo/{id} - Deleta uma tarefa.
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var task = _todoService.GetById(id);

            if (task == null)
            {
                return NotFound();
            }

            _todoService.Delete(id);
            return NoContent();
        }
    }
}