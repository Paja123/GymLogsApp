import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { LoginRequest, RegisterRequest, AuthResponse } from '../models/auth.models';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private readonly apiUrl = 'https://localhost:7247/api/auth';
  private currentUserSignal = signal<AuthResponse | null>(null);
  currentUser = this.currentUserSignal.asReadonly();


  login (payload: LoginRequest): Observable<AuthResponse> {
  return this.http.post<AuthResponse>(`${this.apiUrl}/login`, payload)
  .pipe(catchError(err => {
    console.error('Login error:', err);
    return of({ email: '', username: '' });
  }));
}
  register(payload: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, payload)
      .pipe(catchError(err => {
        console.error('Registration error:', err);
        return of({ email: '', username: '' });
      }));
  }

  logout() {
    return this.http.post(`${this.apiUrl}/logout`, {})
      .pipe(tap(() => this.currentUserSignal.set(null)));
  }

  
  fetchCurrentUser() {
    return this.http.get<AuthResponse>(`${this.apiUrl}/me`)
      .pipe(
        tap(user => this.currentUserSignal.set(user)),
        catchError(() => {
          this.currentUserSignal.set(null);
          return of(null);
        })
      );
    }
  isLoggedIn(): boolean {
    return !!this.currentUser();
  }
}
