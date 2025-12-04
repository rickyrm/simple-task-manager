namespace TaskManager.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; } // PK auto increment
        public string Title { get; set; } = null!; // obligatorio
        public string? Description { get; set; } // opcional
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow; // fecha se establece al crear
    }
}
