import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';

export interface LoginResponse {
  isSuccess: boolean;
  result?: {
    token: string;
    user?: any;
  };
  errorMessages?: string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}/api/v1/usuarios`;
  private tokenKey = 'auth_token';
  
  // Subject para manejar el estado de autenticación
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.checkInitialAuthState());
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(
    private http: HttpClient,
    private router: Router
  ) {}

  /**
   * Verifica el estado inicial de autenticación al cargar la app
   */
  private checkInitialAuthState(): boolean {
    return this.isAuthenticated();
  }

  /**
   * Realiza el login del usuario
   */
  login(credentials: {username: string, password: string}): Observable<LoginResponse> {
    const loginData = {
      NombreUsuario: credentials.username,
      Password: credentials.password
    };

    console.log('URL completa:', `${this.apiUrl}/login`);
    console.log('Datos enviados:', loginData);

    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, loginData)
      .pipe(
        tap(response => {
          console.log('Respuesta completa del API:', response);
          if (response.isSuccess && response.result && response.result.token) {
            this.setToken(response.result.token);
            this.isAuthenticatedSubject.next(true);
            console.log('Token guardado:', response.result.token);
          }
        })
      );
  }

  /**
   * Guarda el token en localStorage
   */
  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  /**
   * Obtiene el token del localStorage
   */
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  /**
   * Verifica si el usuario está autenticado
   */
  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const exp = payload.exp * 1000; // Convertir a millisegundos
      const isValid = Date.now() < exp;
      
      if (!isValid) {
        // Si el token expiró, limpiarlo
        this.logout();
        return false;
      }
      
      return true;
    } catch (error) {
      console.error('Error al verificar token:', error);
      this.logout();
      return false;
    }
  }

  /**
   * Obtiene información del usuario desde el token
   */
  getUserInfo(): any {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return {
        username: payload.unique_name,
        role: payload.role,
        exp: payload.exp
      };
    } catch (error) {
      console.error('Error al obtener info del usuario:', error);
      return null;
    }
  }

  /**
   * Cierra la sesión del usuario
   */
  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.isAuthenticatedSubject.next(false);
    this.router.navigate(['/login']);
  }

  /**
   * Verifica si el token está próximo a expirar (ej: en los próximos 5 minutos)
   */
  isTokenExpiringSoon(minutesThreshold: number = 5): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const exp = payload.exp * 1000;
      const threshold = minutesThreshold * 60 * 1000;
      return (exp - Date.now()) < threshold;
    } catch {
      return false;
    }
  }
}