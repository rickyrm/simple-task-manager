import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TaskItem } from '../../models/task.model';

@Component({
  selector: 'app-task-modal',
  templateUrl: './task-modal.html',
  styleUrls: ['./task-modal.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class TaskModalComponent implements OnInit {
  @Input() task?: TaskItem;

  /**
   * Reactive form used to capture title/description/isCompleted.
   * Initialized in the constructor with validators for required fields.
   */
  form: any;

  constructor(public activeModal: NgbActiveModal, private fb: FormBuilder) {
    this.form = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      isCompleted: [false]
    });
  }

  ngOnInit(): void {
    if (this.task) {
      this.form.patchValue({
        title: this.task.title,
        description: this.task.description,
        isCompleted: this.task.isCompleted
      });
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    // Construye el objeto TaskItem a partir del formulario.
    // Si se está editando, preserva el `id` y `createdAt` existentes.
    const values = this.form.value || {};
    const result: TaskItem = {
      id: this.task?.id ?? 0,
      title: (values.title ?? '').toString(),
      description: (values.description ?? '').toString(),
      isCompleted: !!values.isCompleted,
      createdAt: this.task?.createdAt ?? new Date().toISOString()
    };

    // Cierra el modal y pasa el resultado al llamador (TaskListComponent).
    this.activeModal.close(result);
  }

  cancel(): void {
    this.activeModal.dismiss();
  }
}

