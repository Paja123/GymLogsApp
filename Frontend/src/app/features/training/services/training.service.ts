import { Injectable } from '@angular/core';
import { TrainingSession } from '../../../shared/models/training-session.model';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class TrainingService {

  private apiUrl = 'https://localhost:7247/api/TrainingSession';

  constructor(private http: HttpClient) { }

  createTrainingSession(training: TrainingSession): Observable<TrainingSession> {
    return this.http.post<TrainingSession>(this.apiUrl, training);
  }
}
