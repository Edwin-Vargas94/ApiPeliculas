//import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
//import { provideRouter } from '@angular/router';

//import { routes } from './app.routes';

//export const appConfig: ApplicationConfig = {
//  providers: [
//    provideBrowserGlobalErrorListeners(),
//    provideZoneChangeDetection({ eventCoalescing: true }),
//    provideRouter(routes)
//  ]
//};

import { provideRouter, Routes } from '@angular/router';
import { routes as peliculasRoutes } from './features/peliculas/peliculas.routes';

export const routes: Routes = [
  {
    path: 'peliculas',
    loadChildren: () => import('./features/peliculas/peliculas.routes')
      .then(m => m.routes),
  },
  { path: '', redirectTo: 'peliculas', pathMatch: 'full' }
];

export const appConfig = {
  providers: [provideRouter(routes)],
};
