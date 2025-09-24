import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  // Inyectar el AuthService
  const authService = inject(AuthService);
  
  // Obtener el token del AuthService
  const token = authService.getToken();
  
  // Si hay token y la request no es al endpoint de login
  if (token && !req.url.includes('/login')) {
    // Clonar la request y agregar el header de autorización
    const authReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
    
    return next(authReq);
  }
  
  // Si no hay token o es login, enviar la request original
  return next(req);
};