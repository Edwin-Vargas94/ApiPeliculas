import { bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { appConfig } from './app/app.config';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideRouter } from '@angular/router';
import { routes } from './app/features/peliculas/peliculas.routes';
import { FormsModule } from '@angular/forms';
import { importProvidersFrom } from '@angular/core';
import { authInterceptor } from './app/interceptors/auth.interceptor';

bootstrapApplication(App, {
  providers: [
    //...appConfig.providers,
    provideRouter(routes),
    provideHttpClient(
      withInterceptors([authInterceptor])
    ), // 👈 necesario para usar HttpClient
    importProvidersFrom(FormsModule)
  ],
}).catch(err => console.error(err));
