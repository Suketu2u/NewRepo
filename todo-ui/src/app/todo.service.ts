import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToDoItem } from './todo.model';
import { environment } from '../environment/environment';

@Injectable({
  providedIn: 'root',
})
export class ToDoService {
  // Fetch API base URL from environment settings
  private apiUrl = environment.apiBaseUrl + '/todo';


  constructor(private http: HttpClient) {}

  // Fetch all to-dos
  getToDos(): Observable<ToDoItem[]> {
    return this.http.get<ToDoItem[]>(`${this.apiUrl}`).pipe(
      catchError(this.handleError)
    );
  }

  // Delete a to-do by name
  deleteToDo(name: string): Observable<void> {
    const url = `${this.apiUrl}/${encodeURIComponent(name)}`;
    return this.http.delete<void>(url).pipe(catchError(this.handleError));
  }

  // Add a to-do
  addToDo(toDo: ToDoItem): Observable<ToDoItem> {
    return this.http.post<ToDoItem>(`${this.apiUrl}`, toDo).pipe(
      catchError(this.handleError)
    );
  }

  // Edit a to-do
  editToDo(toDo: ToDoItem): Observable<ToDoItem> {
    return this.http.put<ToDoItem>(`${this.apiUrl}`, toDo).pipe(
      catchError(this.handleError)
    );
  }

  // Error handling method
  private handleError(error: HttpErrorResponse) {
    // Handle specific error cases
    if (error.error instanceof ErrorEvent) {
      console.error('An error occurred:', error.error.message); // Client-side error
    } else {
      console.error(
        `Backend returned code ${error.status}, body was: ${error.error}`
      ); // Backend error
    }
    return throwError(error.error);
  }
}
