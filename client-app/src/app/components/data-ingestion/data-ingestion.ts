import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DataIngestionService } from '../../services/data-ingestion.service';
import { Measure, IngestionDataResponse } from '../../models/data-ingestion.model';

@Component({
  selector: 'app-data-ingestion',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './data-ingestion.component.html',
  styleUrls: ['./data-ingestion.component.css']
})
export class DataIngestionComponent implements OnInit {
  uploadForm!: FormGroup;
  measures: Measure[] = [];
  selectedFile: File | null = null;
  
  // נתוני הטבלה הדינמית
  columns: string[] = [];
  tableData: Record<string, any>[] = [];
  isLoading = false;
  selectedMeasureIdForView: number | null = null;
  searchTerm = '';

  constructor(
    private fb: FormBuilder,
    private ingestionService: DataIngestionService
  ) {}

  ngOnInit(): void {
    // אתחול טופס Reactive עם ולידציות
    this.uploadForm = this.fb.group({
      measureId: ['', Validators.required],
      year: [new Date().getFullYear(), [Validators.required, Validators.min(2000)]],
      period: ['', Validators.required],
      file: [null, Validators.required]
    });

    this.loadMeasures();
  }

  loadMeasures(): void {
    this.ingestionService.getMeasures().subscribe({
      next: (data) => this.measures = data,
      error: (err) => console.error('שגיאה שטעינת מדדים:', err)
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.uploadForm.patchValue({ file: this.selectedFile });
    }
  }

  onUpload(): void {
    if (this.uploadForm.invalid || !this.selectedFile) {
      return;
    }

    const { measureId, year, period } = this.uploadForm.value;
    this.isLoading = true;

    this.ingestionService.uploadExcel(measureId, year, period, this.selectedFile).subscribe({
      next: () => {
        alert('הקובץ נקלט בהצלחה!');
        this.isLoading = false;
        this.selectedMeasureIdForView = measureId;
        this.fetchData(measureId);
      },
      error: (err) => {
        alert('שגיאה בקליטת הקובץ');
        console.error(err);
        this.isLoading = false;
      }
    });
  }

  onMeasureViewChange(event: Event): void {
    const select = event.target as HTMLSelectElement;
    const measureId = Number(select.value);
    if (measureId) {
      this.selectedMeasureIdForView = measureId;
      this.fetchData(measureId);
    }
  }

  fetchData(measureId: number): void {
    this.isLoading = true;
    this.ingestionService.getIngestedData(measureId, 1, 20, this.searchTerm).subscribe({
      next: (res: IngestionDataResponse) => {
        this.columns = res.columns;
        this.tableData = res.rows;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('שגיאה בטעינת נתונים:', err);
        this.isLoading = false;
      }
    });
  }

  onSearch(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.searchTerm = input.value;
    if (this.selectedMeasureIdForView) {
      this.fetchData(this.selectedMeasureIdForView);
    }
  }
}