import { Routes } from '@angular/router';
//import { Routes, RouterModule } from '@angular/router';
//import { ModuleWithProviders } from '@angular/core';

//EGVG 07/08/25: Importa los componentes que vas a usar en las rutas
import { HomeComponent } from './pages/home/home.component';
import { ListaPeliculasComponent } from './pages/lista-peliculas/lista-peliculas.component';
import { CrearPeliculaComponent } from './pages/crear-pelicula/crear-pelicula.component';
import { EditarPeliculaComponent } from './pages/editar-pelicula/editar-pelicula.component';

export const routes: Routes = [
  //{ path: '', component: ListaPeliculasComponent },
  { path: 'lista-peliculas', component: ListaPeliculasComponent },
  { path: '', redirectTo: 'pagina-principal', pathMatch: 'full' },
  { path: 'pagina-principal', component: HomeComponent },
  { path: 'crear', component: CrearPeliculaComponent },
  { path: 'editar/:id', component: EditarPeliculaComponent }
];

//export const appRoutingProviders: any[] = [];
//export const routing: ModuleWithProviders<RouterModule> = RouterModule.forRoot(routes);