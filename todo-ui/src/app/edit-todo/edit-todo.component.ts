import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-todo',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './edit-todo.component.html',
  styleUrl: './edit-todo.component.css'
})
export class EditTodoComponent {

  @Input() toDo: any; // Input to pass the task to edit
  @Input() editMode: boolean = false; // Determines if it's editing or adding
  @Output() save: EventEmitter<any> = new EventEmitter(); // Event to emit changes
  @Output() close: EventEmitter<void> = new EventEmitter(); // Event to close modal

  showModal: any;
  modalErrorMessage: string | undefined;

closeModal() {
    this.close.emit();
  }

  saveChanges(): void {
    // Reset previous errors
    this.modalErrorMessage = '';
  
    // Client-side validation
    if (!this.toDo.name) {
      this.modalErrorMessage = 'Name is required.';
      return;
    }
  
    if (this.toDo.priority <= 0) {
      this.modalErrorMessage = 'Priority must be greater than 0.';
      return;
    }
      
    // Emit the valid To-Do to parent
    this.save.emit(this.toDo);
    this.closeModal();
  }
}
