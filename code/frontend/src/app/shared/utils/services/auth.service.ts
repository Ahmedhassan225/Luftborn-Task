// src/app/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { User } from '../models';

@Injectable({
  providedIn: 'root'
})

export class AuthService {
  private apiUrl = 'http://localhost:5073/api';
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {
    // Check for user info in localStorage on startup
    const storedUser = localStorage.getItem('currentUser');
    if (storedUser) {
      this.currentUserSubject.next(JSON.parse(storedUser));
    }
    
    // Check URL for auth params (after redirect)
    this.checkUrlForAuth();
  }

  private checkUrlForAuth(): void {
    const urlParams = new URLSearchParams(window.location.search);
    const auth = urlParams.get('auth');
    
    if (auth) {
      try {
        const user = JSON.parse(auth);
        this.setCurrentUser(user);
        
        // Clean up URL
        window.history.replaceState({}, document.title, window.location.pathname);
      } catch (error) {
        console.error('Error parsing auth data:', error);
      }
    }
  }

  login(): void {
    // Redirect to the backend auth endpoint
    window.location.href = `${this.apiUrl}/Auth/login?returnUrl=${window.location.origin}`;
  }

  logout(): Observable<any> {
    return this.http.post(`${this.apiUrl}/Auth/logout`, {}).pipe(
      tap(() => {
        this.clearCurrentUser();
      })
    );
  }

  getCurrentUser(): Observable<User | null> {
    if (this.currentUserSubject.value) {
      return of(this.currentUserSubject.value);
    }

    return this.http.get<User>(`${this.apiUrl}/Auth/user`).pipe(
      tap(user => {
        if (user) {
          this.setCurrentUser({ ...user, isAuthenticated: true });
        }
      }),
      catchError(() => {
        return of(null);
      })
    );
  }

  get isLoggedIn(): boolean {
    return !!this.currentUserSubject.value;
  }

  private setCurrentUser(user: User): void {
    localStorage.setItem('currentUser', JSON.stringify(user));
    this.currentUserSubject.next(user);
  }

  private clearCurrentUser(): void {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }
}