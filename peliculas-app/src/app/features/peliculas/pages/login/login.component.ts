import { Component } from '@angular/core';
import { Router, RouterLink, ActivatedRoute } from '@angular/router';
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
  returnUrl = '/'; // URL a la que redirigir después del login

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    // Obtener la URL de retorno desde los query params
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

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
          // Navegar a la URL de retorno sin recargar la página
          this.router.navigate([this.returnUrl]);
        } else {
          this.errorMessage = response.errorMessages?.[0] || 'Error en el login';
        }
      },
      error: (error) => {
        console.error('Error en login:', error);
        if (error.status === 400 && error.error) {
          if (error.error.errorMessages && error.error.errorMessages.length > 0) {
            this.errorMessage = error.error.errorMessages[0];
          } else {
            this.errorMessage = 'Usuario o contraseña incorrectos';
          }
        } else if (error.status === 401) {
          this.errorMessage = 'Credenciales inválidas';
        } else if (error.status === 0) {
          this.errorMessage = 'Error de conexión. Verifica tu conexión a internet.';
        } else {
          this.errorMessage = 'Error de conexión. Intenta nuevamente.';
        }
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  onInputChange() {
    // Limpiar mensajes de error cuando el usuario empiece a escribir
    if (this.errorMessage) {
      this.errorMessage = '';
    }
  }
}