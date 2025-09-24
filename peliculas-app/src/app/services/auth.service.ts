import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // ESTA era la línea correcta, pero parece que se cambió
  private apiUrl = `${environment.apiUrl}/api/v1/usuarios`; 
  private tokenKey = 'auth_token';

  constructor(private http: HttpClient) {}

  login(credentials: {username: string, password: string}): Observable<any> {
    const loginData = {
      NombreUsuario: credentials.username,
      Password: credentials.password
    };

    console.log('URL completa:', `${this.apiUrl}/login`);
    console.log('Datos enviados:', loginData);

    // Aquí debe ser /usuarios/login, no /auth/login
    return this.http.post<any>(`${this.apiUrl}/login`, loginData)
      .pipe(
        tap(response => {
          console.log('Respuesta completa del API:', response);
          
          if (response.isSuccess && response.result && response.result.token) {
            localStorage.setItem(this.tokenKey, response.result.token);
            console.log('Token guardado:', response.result.token);
          }
        })
      );
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;
    
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const exp = payload.exp * 1000;
      return Date.now() < exp;
    } catch {
      return false;
    }
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }
}