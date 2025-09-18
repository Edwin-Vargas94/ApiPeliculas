import { Routes } from '@angular/router';

export const routes: Routes = [

  // Ruta por defecto
  { 
    path: '', 
    redirectTo: '/peliculas', 
    pathMatch: 'full' 
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
