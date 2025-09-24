import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PeliculaService, Pelicula } from '../../../../services/pelicula.service';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginatorModule } from '@angular/material/paginator';
import { RouterLink } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ViewChild } from '@angular/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  standalone: true,
  selector: 'app-lista-peliculas',
  imports: [CommonModule, MatButtonModule,
    MatPaginatorModule, RouterLink,
    MatIconModule, MatTooltipModule, MatSnackBarModule, MatProgressSpinnerModule, 
    MatFormFieldModule, MatInputModule, MatTableModule],
  templateUrl: './lista-peliculas.component.html',
  styleUrls: ['./lista-peliculas.component.scss']
  
})
export class ListaPeliculasComponent implements OnInit {
  displayedColumns: string[] = ['id', 'nombre', 'descripcion', 'duracion', 'clasificacion', 'fechaCreacion', 'imagen', 'acciones'];
  dataSource = new MatTableDataSource<Pelicula>();
  isLoading = true;
  paginaActual = 1;
  totalPaginas = 1;
  apiUrl = 'https://apipeliculas-rtsd.onrender.com'; // Reemplaza con la URL real de tu API

  pelicula: Pelicula = {
    id: 0,
    nombre: '',
    descripcion: '',
    duracion: 0,
    rutaImagen: '',
    clasificacion: 0,
    fechaCreacion: '',
    categoriaID: 0
  };

  archivoSeleccionado: File | null = null;


  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  constructor(
    private peliculaService: PeliculaService,
    private snackBar: MatSnackBar,
    private http: HttpClient
  ) {}

  ngOnInit(): void {
    this.cargarPeliculas();
  }

  cargarPeliculas(): void {
    this.isLoading = true;
    this.peliculaService.obtenerPeliculas(1, 100).subscribe({
      next: (data: any) => {
        this.dataSource.data = data.items ?? [];
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.isLoading = false;
        this.snackBar.open('Películas cargadas correctamente 🎉', 'Cerrar', { duration: 3000 });
      },
      error: (err) => {
        console.error('Error al obtener películas:', err);
        this.isLoading = false;
        this.snackBar.open('Error al cargar películas 😢', 'Cerrar', { duration: 3000 });
      }
    });
  }

  aplicarFiltro(event: Event) {
    const valor = (event.target as HTMLInputElement).value;
    this.dataSource.filter = valor.trim().toLowerCase();
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

// En lista-peliculas.component.ts
eliminarPelicula(id: number): void {
  // Opcional: Mostrar confirmación antes de eliminar
  if (confirm('¿Estás seguro de que deseas eliminar esta película?')) {
    
    this.peliculaService.eliminarPelicula(id).subscribe({
      next: (response) => {
        console.log('Película eliminada exitosamente:', response);
        
        // Mostrar mensaje de éxito
        this.snackBar.open('Película eliminada correctamente ✅', 'Cerrar', { 
          duration: 3000 
        });
        
        // Recargar la lista de películas
        this.cargarPeliculas();
      },
      error: (error) => {
        console.error('Error al eliminar película:', error);
        
        // Mostrar mensaje de error
        this.snackBar.open('Error al eliminar la película ❌', 'Cerrar', { 
          duration: 3000 
        });
      }
    });
  }
}

}

