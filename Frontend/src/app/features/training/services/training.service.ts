import { Injectable } from '@angular/core';
import { TrainingSession } from '../../../shared/models/training-session.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { TrainingSessionResponseDto } from '../../../shared/models/training-session-response-dto';

@Injectable({
  providedIn: 'root',
})
export class TrainingService {

  private apiUrl = 'https://localhost:7247/api/TrainingSession';

  constructor(private http: HttpClient) { }

  createTrainingSession(training: TrainingSession): Observable<TrainingSession> {
    return this.http.post<TrainingSession>(this.apiUrl, training);
  }
  getAll(): Observable<TrainingSessionResponseDto[]> {
    return this.http.get<TrainingSessionResponseDto[]>(this.apiUrl);
  }
  delete(id: string): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${id}`);
  }
}