import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataIngestion } from './data-ingestion';

describe('DataIngestion', () => {
  let component: DataIngestion;
  let fixture: ComponentFixture<DataIngestion>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DataIngestion],
    }).compileComponents();

    fixture = TestBed.createComponent(DataIngestion);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
