namespace TaskManager.Domain.Entities
{
    public class TaskItem
    {
        // Identificador de la entidad (clave primaria en la BD).
        public int Id { get; set; } // PK auto increment

        // Título obligatorio de la tarea. Las restricciones (max length, required)
        // se aplican tanto en el DbContext como en los DTOs para validación.
        public string Title { get; set; } = null!; // obligatorio

        // Descripción opcional, nula por defecto.
        public string? Description { get; set; } // opcional

        // Indica si la tarea se ha completado.
        public bool IsCompleted { get; set; } = false;

        // Fecha de creación. Setter privado para evitar modificaciones desde fuera
        // y garantizar inmutabilidad después de la creación (a menos que se use reflexión).
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow; // fecha se establece al crear
    }
}
