using System.ComponentModel.DataAnnotations;

namespace TaskManager.Application.DTOs
{
    /// <summary>
    /// DTO para la creación de una tarea.
    /// Contiene los campos que el cliente puede enviar al crear una tarea.
    /// </summary>
    public class TaskCreateDto
    {
        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(200, ErrorMessage = "El título no puede superar 200 caracteres.")]
        public string Title { get; set; } = null!;

        [StringLength(1000, ErrorMessage = "La descripción no puede superar 1000 caracteres.")]
        public string? Description { get; set; }
    }
}
