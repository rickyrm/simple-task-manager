import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TaskModalComponent } from '../task-modal/task-modal';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Task } from '../../services/task';
import { TaskItem } from '../../models/task.model';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.html',
  styleUrls: ['./task-list.scss'],
  standalone: true,
  imports: [CommonModule, FormsModule, TaskModalComponent]
})
/**
 * TaskListComponent
 * - Carga la lista de tareas desde el backend usando el servicio `Task`.
 * - Mantiene `tasks` (todas las tareas) y `filtered` (según el filtro seleccionado).
 * - Soporta operaciones CRUD: crear, editar, borrar y toggle de completado.
 * - Después de cada operación asíncrona fuerza la detección de cambios para
 *   que la vista se actualice inmediatamente.
 */
export class TaskListComponent implements OnInit {
  tasks: TaskItem[] = [];
  filtered: TaskItem[] = [];
  loading = false;
  error = '';
  filter: 'all' | 'active' | 'completed' = 'all';

  constructor(private taskService: Task, private modalService: NgbModal, private cd: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = '';
    // Realiza petición HTTP para obtener todas las tareas
    this.taskService.getTasks().subscribe({
      next: (res) => {
        this.tasks = res;
        this.applyFilter();
        this.loading = false;
        // Detecta los cambios para actualizar la vista
        try { this.cd.detectChanges(); } catch {}
      },
      error: (err) => {
        this.error = 'Error cargando tareas';
        this.loading = false;
        console.error(err);
      }
    });
  }

  applyFilter(): void {
    if (this.filter === 'all') this.filtered = [...this.tasks];
    else if (this.filter === 'active') this.filtered = this.tasks.filter(t => !t.isCompleted);
    else this.filtered = this.tasks.filter(t => t.isCompleted);
  }

  setFilter(f: 'all' | 'active' | 'completed') {
    this.filter = f;
    this.applyFilter();
  }

  openCreate(): void {
    const ref = this.modalService.open(TaskModalComponent, { centered: true });
    ref.result.then((res: TaskItem) => {
      this.taskService.createTask(res).subscribe({
        next: (created) => {
          this.tasks.push(created);
          this.applyFilter();
          if (created && created.id != null) this.highlightAndScroll(created.id);
          try { this.cd.detectChanges(); } catch {}
        },
        error: (e) => { this.error = 'Error creando tarea'; console.error(e); }
      });
    }).catch(() => {});
  }

  openEdit(task: TaskItem): void {
    const ref = this.modalService.open(TaskModalComponent, { centered: true });
    ref.componentInstance.task = { ...task };
    ref.result.then((payload: TaskItem) => {
      this.taskService.updateTask(payload).subscribe({
        next: (updated) => {
          const server = updated ?? payload;
          const idx = this.tasks.findIndex(t => t.id === server.id);
          if (idx > -1) this.tasks[idx] = server;
          this.applyFilter();
          if (server && server.id != null) this.highlightAndScroll(server.id);
          try { this.cd.detectChanges(); } catch {}
        },
        error: (e) => { this.error = 'Error actualizando tarea'; console.error(e); }
      });
    }).catch(() => {});
  }

  toggleComplete(task: TaskItem): void {
    const updated = { ...task, isCompleted: !task.isCompleted };
    this.taskService.updateTask(updated).subscribe({
      next: (res) => { const server = res ?? updated; const idx = this.tasks.findIndex(t => t.id === server.id); if (idx > -1) this.tasks[idx] = server; this.applyFilter(); if (server && server.id != null) this.highlightAndScroll(server.id); try { this.cd.detectChanges(); } catch {} },
      error: (e) => { this.error = 'Error actualizando estado'; console.error(e); }
    });
  }

  deleteTask(task: TaskItem): void {
    if (!confirm('¿Eliminar esta tarea?')) return;
    this.taskService.deleteTask(task.id).subscribe({
      next: () => { this.tasks = this.tasks.filter(t => t.id !== task.id); this.applyFilter(); },
      error: (e) => { this.error = 'Error borrando tarea'; console.error(e); }
    });
  }

  private highlightAndScroll(id: number): void {
    // Ensure item is visible in current filter; if not, switch to 'all'
    const existsInFiltered = this.filtered.some(t => t.id === id);
    if (!existsInFiltered) {
      this.setFilter('all');
    }

    // Wait a tick so DOM updates and then scroll
    setTimeout(() => {
      const el = document.getElementById(`task-${id}`);
      if (!el) return;
      try { el.scrollIntoView({ behavior: 'smooth', block: 'center' }); } catch { el.scrollIntoView(); }
      el.classList.add('flash');
      setTimeout(() => el.classList.remove('flash'), 2000);
    }, 50);
  }
}

