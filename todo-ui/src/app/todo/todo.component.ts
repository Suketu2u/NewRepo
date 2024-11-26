import { Component, OnInit } from '@angular/core';
import { status, ToDoItem } from '../todo.model';
import { ToDoService } from '../todo.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { EditTodoComponent } from "../edit-todo/edit-todo.component";

@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [CommonModule, FormsModule, EditTodoComponent],
  templateUrl: './todo.component.html',
  styleUrl: './todo.component.css'
})
export class ToDoComponent implements OnInit {
  toDos: ToDoItem[] = [];
  errorMessage: string = '';
  status = status; 
  public selectedToDo: any = null; // Store selected task to edit
  public showModal: boolean = false; // Control modal visibility
  editMode = false; // Determines if it's editing or adding

  constructor(private toDoService: ToDoService) {}

  ngOnInit(): void {
    this.loadToDos();
  }

  loadToDos(): void {
    this.toDoService.getToDos().subscribe({
      next: (data) => {
        this.toDos = data;
        if (this.toDos.length === 0) {
          this.errorMessage = 'No tasks available. Add your first to-do!';
        } else {
          this.errorMessage = '';
        }
      },
      error: (error) => this.errorMessage =  error || 'Error loading todos.'
    });
  }

  addToDo(newToDo: ToDoItem): void {
    
    this.toDoService.addToDo(newToDo).subscribe({
      next: (toDo) => {
        this.toDos.push(toDo);
        this.toDos.sort((a,b)=> a.priority - b.priority).sort((a,b)=> b.status.valueOf() - a.status.valueOf());
        this.errorMessage = '';
      },
      error: (error) => this.errorMessage =  error || 'Error adding todo.'
    });
  }

  deleteToDo(name: string): void {
    this.toDoService.deleteToDo(name).subscribe({
      next: () => {
        this.toDos = this.toDos.filter(toDo => toDo.name !== name);
        this.errorMessage = '';
      },
      error: (error) => {
        this.errorMessage = error || 'Error deleting the todo. Only completed todos can be deleted or the todo may not exist.';
      }
    });
  }
  
  editToDo(updatedToDo: ToDoItem): void {
    this.toDoService.editToDo(updatedToDo).subscribe({
      next: (toDo) => {
        const index = this.toDos.findIndex(t => t.name.toLowerCase() === updatedToDo.name.toLowerCase());
        if (index !== -1) {
          this.toDos[index] = toDo;
          this.toDos.sort((a,b)=> a.priority - b.priority).sort((a,b)=> b.status.valueOf() - a.status.valueOf());
        }
        this.errorMessage = '';
      },
      error: (error) => this.errorMessage =  error || 'Error updating todo.'
    });
  }
  
  getStatusString(statusValue: number): string {
    const statusString = this.status[statusValue];
    return statusString ? statusString : 'Unknown Status';
  }
   // Opens modal for editing
   openEditModal(toDo: any) {
    this.editMode = true;
    this.selectedToDo = { ...toDo }; // Clone the selected To-Do
    this.showModal = true;
  }

  // Opens modal for adding
  openAddModal() {
    this.editMode = false;
    this.selectedToDo = { name: '', priority: '', status: 0 }; // Reset for new To-Do
    this.showModal = true;
  }

  // Closes modal
  closeModal() {
    this.showModal = false;
  }

  // Save changes to the task
  saveToDoChanges(updatedToDo: ToDoItem) {
    // Client-side validation
    if (!updatedToDo.name) {
      this.errorMessage = 'Name is required.';
      return;
    }
    if (updatedToDo.priority <= 0) {
      this.errorMessage = 'Priority must be greater than 0.';
      return;
    }
    if (this.toDos.some(t => t.name.toLowerCase() === updatedToDo.name.toLowerCase() && !this.editMode)) {
      this.errorMessage = 'A task with this name already exists.';
      return;
    }
  
    // Add or Edit logic
    if (this.editMode) {
      this.editToDo(updatedToDo);
    } else {
      this.addToDo(updatedToDo);
    }
  
    // Handle server-side errors
    if (!this.errorMessage) {
      this.errorMessage = '';
      this.closeModal();
    } else {
      this.errorMessage = this.errorMessage;
    }
  }
  
}
