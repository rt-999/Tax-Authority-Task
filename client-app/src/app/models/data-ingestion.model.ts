export interface Measure {
  id: number;
  name: string;
}

export interface IngestionDataResponse {
  columns: string[];
  rows: Record<string, any>[];
  totalCount: number;
}