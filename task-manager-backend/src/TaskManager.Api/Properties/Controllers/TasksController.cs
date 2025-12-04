using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Services;

namespace TaskManager.Api.Controllers
{
    // Controller REST que expone los endpoints de tareas.
    // Nota: los controllers deben ser delgados — delegan la mayor parte
    // de la lógica a la capa de Application (`TaskService`).
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
        /// - Valida parámetros de consulta
        /// - Devuelve DTOs listos para el cliente
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
        /// - 200: devuelve la tarea
        /// - 404: no existe
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
        /// Crea una nueva tarea. Valida el modelo usando DataAnnotations.
        /// - 201: creado con `Location` apuntando a GET /api/tasks/{id}
        /// - 400: modelo inválido
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
        /// - 204: actualizado correctamente
        /// - 400: modelo inválido
        /// - 404: no existe
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
        /// - 204: eliminado
        /// - 404: no existe
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
