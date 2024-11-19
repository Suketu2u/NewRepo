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
    if (!newToDo.name) {
      this.errorMessage = 'Please provide a name for the todo.';
      return;
    }
    // if(this.toDos.findIndex(t => t.name.toLowerCase() === newToDo.name.toLowerCase()) <= 0 ){
    //   this.errorMessage = 'Provided name for the todo is already exist in list.';
    //   return;
    // }
    this.toDoService.addToDo(newToDo).subscribe({
      next: (toDo) => {
        this.toDos.push(toDo);
        this.toDos.sort((a,b)=>a.priority-b.priority).sort((a,b)=>b.status.valueOf() - a.status.valueOf());
        //this.newToDo = {name: '', priority: 1, status: status.NotStarted }; // Reset the form
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
          this.toDos.sort((a,b)=>a.priority-b.priority).sort((a,b)=>b.status.valueOf() - a.status.valueOf());
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
    if (this.editMode) {
      this.editToDo(updatedToDo);
    } else {
      this.addToDo(updatedToDo);
    }
    this.closeModal();
  }
}
