import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PeliculaService, Pelicula } from '../../pelicula.service';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginatorModule } from '@angular/material/paginator';
import { HomeComponent } from "../home/home.component";
import { EmpleadpPruebaComponent } from "../empleado-prueba/empleado-prueba.component";


@Component({
  standalone: true,
  selector: 'app-lista-peliculas',
  imports: [CommonModule, MatButtonModule,
    MatPaginatorModule, HomeComponent, EmpleadpPruebaComponent],
  templateUrl: './lista-peliculas.component.html',
  styleUrls: ['./lista-peliculas.component.scss']
  
})
export class ListaPeliculasComponent implements OnInit {
  peliculas: Pelicula[] = [];
  paginaActual = 1;
  totalPaginas = 1;
  pageSize = 2; // Puedes ajustar esto según tu preferencia

  constructor(private peliculaService: PeliculaService) {}

  ngOnInit(): void {
    this.cargarPeliculas();
  }

  cargarPeliculas(): void {
    this.peliculaService.obtenerPeliculas(this.paginaActual, this.pageSize).subscribe({
      next: (data: any) => {
        console.log('Respuesta de API:', data);
        this.peliculas = data.items ?? [];
        this.totalPaginas = data.totalPages;
      },
      error: (err) => {
        console.error('Error al obtener películas:', err);
        this.peliculas = [];
      }
    });
  }

  paginaSiguiente(): void {
    if (this.paginaActual < this.totalPaginas) {
      this.paginaActual++;
      this.cargarPeliculas();
    }
  }

  paginaAnterior(): void {
    if (this.paginaActual > 1) {
      this.paginaActual--;
      this.cargarPeliculas();
    }
  }
}
