namespace TaskManager.Application.DTOs
{
    /// <summary>
    /// DTO usado para devolver información de una tarea al cliente.
    /// Incluye campos de solo lectura como Id y CreatedAt.
    /// </summary>
    public class TaskReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
