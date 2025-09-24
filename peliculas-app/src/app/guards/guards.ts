import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    
    if (this.authService.isAuthenticated()) {
      // Opcional: verificar si el token está próximo a expirar
      if (this.authService.isTokenExpiringSoon()) {
        console.warn('Token próximo a expirar');
        // Aquí podrías implementar lógica para refrescar el token
      }
      return true;
    }

    // Redirigir al login si no está autenticado
    this.router.navigate(['/login'], { 
      queryParams: { returnUrl: state.url } 
    });
    return false;
  }
}

// Guard alternativo para rutas que solo pueden acceder usuarios no autenticados (como login)
@Injectable({
  providedIn: 'root'
})
export class NoAuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {
    if (!this.authService.isAuthenticated()) {
      return true;
    }

    // Si ya está autenticado, redirigir al home
    this.router.navigate(['/']);
    return false;
  }
}