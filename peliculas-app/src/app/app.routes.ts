import { Routes } from '@angular/router';
import { LoginComponent } from './features/peliculas/pages/login/login.component';
import { AuthGuard, NoAuthGuard } from './guards/guards';

export const routes: Routes = [

  // Ruta por defecto
  { 
    path: '', 
    redirectTo: '/peliculas', 
    pathMatch: 'full' 
  },
  
  
   {
    path: 'login',
    component: LoginComponent,
    canActivate: [NoAuthGuard]
  },
  // Rutas de películas (lazy loading)
  {
    path: 'peliculas',
    loadChildren: () => import('./features/peliculas/peliculas.routes').then(m => m.routes)
  },
  
  // Ruta para página no encontrada
  { 
    path: '**', 
    //loadComponent: () => import('./shared/components/editar-pelicula/editar-pelicula.component.ts').then(c => c.NotFoundComponent)
  }
];
