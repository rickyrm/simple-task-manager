/**
 * Representa una tarea en el frontend.
 * - `id`: identificador único asignado por el backend
 * - `title`: título de la tarea (requerido en el backend)
 * - `description`: detalle opcional de la tarea
 * - `isCompleted`: estado de completado
 * - `createdAt`: fecha ISO de creación (string en frontend)
 */
export interface TaskItem {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: string;
}

