export interface ToDoItem {
    //id: string;
    name: string;
    priority: number;
    status: status;
  }
  export enum status{
    NotStarted = 0,//'NotStarted',
    InProgress = 1,//'InProgress',
    Completed = 2//'Completed'
  }