import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaskItem } from '../models/task.model';

@Injectable({
  providedIn: 'root'
})
export class Task {
  /**
   * URL base del API. Para desarrollo se apunta a localhost:5121.
   * Para producción, reemplazar por la ruta pública del API o usar
   * proxy.conf.json para ocultar el host real.
   */
  private baseUrl = 'http://localhost:5121/api/tasks'; // ajusta el puerto si es necesario

  constructor(private http: HttpClient) { }

  getTasks(): Observable<TaskItem[]> {
    // GET /api/tasks -> devuelve array de TaskItem
    return this.http.get<TaskItem[]>(this.baseUrl);
  }

  createTask(task: TaskItem): Observable<TaskItem> {
    // POST /api/tasks -> crea una tarea y suele devolver la tarea creada con id
    return this.http.post<TaskItem>(this.baseUrl, task);
  }

  updateTask(task: TaskItem): Observable<TaskItem> {
    // PUT /api/tasks/{id} -> actualiza la tarea. Algunos endpoints devuelven
    // el recurso actualizado; otros pueden devolver 204 No Content.
    return this.http.put<TaskItem>(`${this.baseUrl}/${task.id}`, task);
  }

  deleteTask(id: number): Observable<void> {
    // DELETE /api/tasks/{id} -> borra la tarea
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}

