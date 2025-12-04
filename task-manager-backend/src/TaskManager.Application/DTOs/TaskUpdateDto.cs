using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs
{
    /// <summary>
    /// DTO para actualizar una tarea existente.
    /// Solo Title, Description e IsCompleted son editables.
    /// </summary>
    public class TaskUpdateDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede superar 200 caracteres.")]
        public string Title { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "La descripción no puede superar 1000 caracteres.")]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
