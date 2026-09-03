import { Component } from '@angular/core';
import { DataIngestionComponent } from './components/data-ingestion/data-ingestion';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [DataIngestionComponent],
  template: `<app-data-ingestion></app-data-ingestion>`
})
export class AppComponent {}