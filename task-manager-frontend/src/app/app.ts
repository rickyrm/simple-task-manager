import { Component } from '@angular/core';
import { TaskListComponent } from './components/task-list/task-list';
import { CommonModule } from '@angular/common';

/**
 * App (root) component
 * - Define layout general de la aplicación (navbar + contenedor central).
 * - Monta el `TaskListComponent` dentro del contenedor principal.
 */
@Component({
  selector: 'app-root',
  template: `
    <div class="app-shell">
      <header class="app-header shadow-sm">
        <nav class="navbar navbar-expand bg-white">
          <div class="container app-container">
            <a class="navbar-brand brand">Task Manager</a>
            <div class="top-actions ms-auto">
              <span class="text-muted small">Local</span>
            </div>
          </div>
        </nav>
      </header>

      <main class="app-main">
        <div class="container app-container">
          <!-- Componente principal que muestra y gestiona las tareas -->
          <app-task-list></app-task-list>
        </div>
      </main>
    </div>
  `,
  standalone: true,
  imports: [CommonModule, TaskListComponent]
})
export class App {}

