using System;

namespace TaskManager.Application.Exceptions
{
	/// <summary>
	/// Excepción usada por la capa de aplicación para señalar errores de validación
	/// que no sean simplemente ModelState (por ejemplo reglas de negocio específicas).
	/// Se puede atrapar en un middleware o en el controller para mapear a 400 Bad Request.
	/// </summary>
	public class ValidationException : Exception
	{
		public ValidationException()
		{
		}

		public ValidationException(string message) : base(message)
		{
		}

		public ValidationException(string message, Exception inner) : base(message, inner)
		{
		}
	}
}