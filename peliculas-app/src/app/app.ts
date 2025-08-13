import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';

import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';
//import { HomeComponent } from './features/peliculas/pages/home/home.component';
//import { EmpleadpPruebaComponent } from './features/peliculas/pages/empleado-prueba/empleado-prueba.component';
//import { appRoutingProviders } from './features/peliculas/peliculas.routes';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    NavbarComponent,
    FooterComponent,
    //HomeComponent,
    //EmpleadpPruebaComponent,
    FormsModule
  ],
  //providers: [appRoutingProviders],
  template: `
    <app-navbar></app-navbar>
    <router-outlet></router-outlet>
    <app-footer></app-footer>
  `,
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('peliculas-app');
}