using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Services
{
    /// <summary>
    /// Servicio que implementa la lógica de negocio de las tareas.
    /// Orquesta la comunicación entre DTOs y repositorio.
    /// </summary>
    public class TaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Obtiene tareas con filtrado y paginación.
        /// </summary>
        public async Task<IEnumerable<TaskReadDto>> GetAllAsync(bool? isCompleted, int page, int pageSize)
        {
            var tasks = await _repository.GetAllAsync(isCompleted, page, pageSize);
            return tasks.Select(t => new TaskReadDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                CreatedAt = t.CreatedAt
            });
        }

        /// <summary>
        /// Obtiene tarea por Id.
        /// </summary>
        public async Task<TaskReadDto?> GetByIdAsync(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null) return null;

            return new TaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }

        /// <summary>
        /// Crea una nueva tarea.
        /// </summary>
        public async Task<TaskReadDto> CreateAsync(TaskCreateDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description
            };

            var created = await _repository.CreateAsync(task);

            return new TaskReadDto
            {
                Id = created.Id,
                Title = created.Title,
                Description = created.Description,
                IsCompleted = created.IsCompleted,
                CreatedAt = created.CreatedAt
            };
        }

        /// <summary>
        /// Actualiza una tarea existente.
        /// </summary>
        public async Task<bool> UpdateAsync(int id, TaskUpdateDto dto)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null) return false;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.IsCompleted = dto.IsCompleted;

            await _repository.UpdateAsync(task);
            return true;
        }

        /// <summary>
        /// Elimina una tarea por Id.
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _repository.GetByIdAsync(id);
            if (task == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}
