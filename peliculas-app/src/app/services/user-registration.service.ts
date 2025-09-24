import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';
import { environment } from '../../environments/environment';

export interface RegistroUsuarioDto {
  nombreUsuario: string;
  nombre: string;
  password: string;
  role: string;
}

export interface RegistroResponse {
  isSuccess: boolean;
  result?: any;
  errorMessages?: string[];
}

@Injectable({
  providedIn: 'root'
})
export class UserRegistrationService {
  private apiUrl = `${environment.apiUrl}/api/v1/usuarios`;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {}

  /**
   * Verifica si el usuario actual es administrador
   */
  isCurrentUserAdmin(): boolean {
    const userInfo = this.authService.getUserInfo();
    return userInfo && userInfo.role === 'Admin';
  }

  /**
   * Registra un nuevo usuario
   */
  registrarUsuario(userData: RegistroUsuarioDto): Observable<RegistroResponse> {
    // El interceptor automáticamente agregará el token de autorización
    return this.http.post<RegistroResponse>(`${this.apiUrl}/registro`, userData);
  }

  /**
   * Obtiene los roles disponibles según el usuario actual
   */
  getRolesDisponibles(): string[] {
    if (this.isCurrentUserAdmin()) {
      return ['Usuario', 'Admin'];
    }
    // Si no es admin, solo puede crear usuarios normales
    return ['Usuario'];
  }
}