import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '@/app/services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-login',
  imports: [RouterLink, FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  credentials = {
    username: '',
    password: ''
  };
  
  isLoading = false;
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onLogin() {
    if (!this.credentials.username || !this.credentials.password) {
      this.errorMessage = 'Por favor completa todos los campos';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.credentials).subscribe({
      next: (response) => {
        console.log('Respuesta del login:', response);
        
        if (response.isSuccess && response.result && response.result.token) {
          console.log('Login exitoso');
          // Navegar al home y recargar la página para actualizar el estado
          this.router.navigate(['/']).then(() => {
            window.location.reload();
          });
        } else {
          this.errorMessage = response.errorMessages?.[0] || 'Error en el login';
        }
      },
      error: (error) => {
        console.error('Error en login:', error);
        
        if (error.status === 400 && error.error) {
          // Manejar errores de validación de tu API
          if (error.error.errorMessages && error.error.errorMessages.length > 0) {
            this.errorMessage = error.error.errorMessages[0];
          } else {
            this.errorMessage = 'Usuario o contraseña incorrectos';
          }
        } else {
          this.errorMessage = 'Error de conexión. Intenta nuevamente.';
        }
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }
}