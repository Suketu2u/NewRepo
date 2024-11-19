import { Component } from '@angular/core';
import { ToDoComponent } from "./todo/todo.component";

@Component({
  selector: 'app-root',
  template: `
    <h1>Welcome to the To-Do Application!</h1>
    <app-todo></app-todo>
  `,
  standalone: true,
  imports: [ToDoComponent],
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title: string = 'To Do App';
}
