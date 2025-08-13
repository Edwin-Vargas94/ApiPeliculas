import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { appConfig } from './app/app.config';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app/features/peliculas/peliculas.routes';

bootstrapApplication(App, {
  providers: [
    //...appConfig.providers,
    provideRouter(routes),
    provideHttpClient() // 👈 necesario para usar HttpClient
  ],
});
