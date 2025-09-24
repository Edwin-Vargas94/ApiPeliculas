import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { UserRegistrationService, RegistroUsuarioDto } from '@/app/services/user-registration.service';
import { AuthService } from '@/app/services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-register-user',
  imports: [RouterLink, FormsModule, CommonModule],
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.scss']
})
export class RegisterUserComponent implements OnInit {
  userData: RegistroUsuarioDto = {
    nombreUsuario: '',
    nombre: '',
    password: '',
    role: ''
  };

  confirmPassword = '';
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  
  currentUserInfo: any = null;
  isAdmin = false;
  rolesDisponibles: string[] = [];

  constructor(
    private userRegistrationService: UserRegistrationService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    // Verificar autenticación
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }

    // Obtener información del usuario actual
    this.currentUserInfo = this.authService.getUserInfo();
    this.isAdmin = this.userRegistrationService.isCurrentUserAdmin();
    this.rolesDisponibles = this.userRegistrationService.getRolesDisponibles();
    
    // Establecer rol por defecto
    if (!this.isAdmin) {
      this.userData.role = 'Usuario';
    }
  }

  onRegister() {
    if (!this.validateForm()) {
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.userRegistrationService.registrarUsuario(this.userData).subscribe({
      next: (response) => {
        console.log('Respuesta del registro:', response);
        if (response.isSuccess) {
          this.successMessage = `Usuario "${this.userData.nombreUsuario}" registrado exitosamente`;
          this.resetForm();
          
          // Opcional: redirigir después de 2 segundos
          setTimeout(() => {
            this.router.navigate(['/peliculas']);
          }, 2000);
        } else {
          this.errorMessage = response.errorMessages?.[0] || 'Error al registrar usuario';
        }
      },
      error: (error) => {
        console.error('Error en registro:', error);
        
        if (error.status === 400 && error.error) {
          if (error.error.errorMessages && error.error.errorMessages.length > 0) {
            this.errorMessage = error.error.errorMessages[0];
          } else {
            this.errorMessage = 'Datos de registro inválidos';
          }
        } else if (error.status === 403) {
          this.errorMessage = 'No tienes permisos para registrar usuarios';
        } else if (error.status === 409) {
          this.errorMessage = 'El nombre de usuario ya existe';
        } else {
          this.errorMessage = 'Error de conexión. Intenta nuevamente.';
        }
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  private validateForm(): boolean {
    if (this.confirmPassword !== this.userData.password) {
      this.errorMessage = 'Las contraseñas no coinciden';
      return false;
    }

    if (!this.isAdmin && this.userData.role === 'Admin') {
      this.errorMessage = 'No tienes permisos para crear usuarios administradores';
      return false;
    }

    return true;
  }

  public resetForm() {
    this.userData = {
      nombreUsuario: '',
      nombre: '',
      password: '',
      role: this.isAdmin ? '' : 'Usuario'
    };
    this.confirmPassword = '';
  }

  onInputChange() {
    if (this.errorMessage) {
      this.errorMessage = '';
    }
    if (this.successMessage) {
      this.successMessage = '';
    }
  }
}