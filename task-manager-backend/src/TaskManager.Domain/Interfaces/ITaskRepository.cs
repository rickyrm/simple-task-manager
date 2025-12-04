using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskRepository
    {
        // Devuelve tareas con filtrado por `isCompleted` y paginación.
        // - `page` es 1-based
        Task<IEnumerable<TaskItem>> GetAllAsync(bool? isCompleted = null, int page = 1, int pageSize = 10);

        // Obtiene por PK. Devuelve null si no existe.
        Task<TaskItem?> GetByIdAsync(int id);

        // Crea la entidad y devuelve la entidad persistida (con Id asignado).
        Task<TaskItem> CreateAsync(TaskItem task);

        // Actualiza la entidad. Debe existir previamente.
        Task UpdateAsync(TaskItem task);

        // Elimina por Id. Implementación concreta puede ser no-op si no existe.
        Task DeleteAsync(int id);

        // Devuelve el total de items (útil para paginación en el API).
        Task<int> CountAsync(bool? isCompleted = null);
    }
}
