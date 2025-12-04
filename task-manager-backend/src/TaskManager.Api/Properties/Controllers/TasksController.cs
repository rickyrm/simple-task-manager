using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly TaskService _taskService;

        public TasksController(TaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// GET /api/tasks?isCompleted=true&page=1&pageSize=10
        /// Lista tareas con filtrado y paginación.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool? isCompleted, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var tasks = await _taskService.GetAllAsync(isCompleted, page, pageSize);
            return Ok(tasks);
        }

        /// <summary>
        /// GET /api/tasks/{id}
        /// Obtiene una tarea por Id.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        /// <summary>
        /// POST /api/tasks
        /// Crea una nueva tarea.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TaskCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _taskService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// PUT /api/tasks/{id}
        /// Actualiza una tarea existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _taskService.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return NoContent();
        }

        /// <summary>
        /// DELETE /api/tasks/{id}
        /// Elimina una tarea por Id.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _taskService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
