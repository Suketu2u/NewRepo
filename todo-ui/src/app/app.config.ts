import { HttpClientModule } from '@angular/common/http';
import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { ToDoService } from './todo.service';

export const appConfig: ApplicationConfig = {
  
  providers: [
    ToDoService,
    importProvidersFrom(BrowserModule, HttpClientModule, FormsModule)
  ],
};
