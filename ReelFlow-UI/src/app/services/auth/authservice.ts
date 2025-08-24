import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap, catchError } from 'rxjs';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { RegisterModel } from '../../models/auth/registerModel.js';
import { LoginModel } from '../../models/auth/loginModel.js';
import { AuthResponse } from '../../models/auth/authResponse.js';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5231/api/ReelFlowAuth'; 
  constructor(private http: HttpClient, private router: Router) { }

  login(loginModel: LoginModel) {
    // console.log('Login model:', loginModel)
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, loginModel)
    .pipe(
      tap((response:AuthResponse) => {
        console.log('token', response.token)
        localStorage.setItem('jwt_token', response.token);
        localStorage.setItem('usernameOrEmail', response.usernameOrEmail);
      })
    );
  }

  register(registerModel: RegisterModel): Observable<any> 
  {
    return this.http.post<any>(`${this.apiUrl}/register`, registerModel).pipe(
      tap((response: any) => {
        console.log('Registration successful:', response);
      }),
      catchError((error: any) => {
        console.error('Registration failed:', error);
        return throwError(() => error);
      })
    );
  }

  isAuthenticated(): boolean {
    return this.hasToken();
  }
  
  private hasToken(): boolean {
    const token = this.getToken();
    return !!token;
  }
  getToken()
  {
    return localStorage.getItem('jwt_token');
  }
  logout(): void {
    localStorage.removeItem('jwt_token');
    localStorage.removeItem('usernameOrEmail');
    this.router.navigate(['/login']); 
  }

  getCurrentUser(){
    return localStorage.getItem('usernameOrEmail');
  }

}
