import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ListaPeliculasComponent } from "../lista-peliculas/lista-peliculas.component";
import { RouterOutlet } from '@angular/router';
import { PeliculaService, Pelicula } from '../../pelicula.service';

// EGVG 07/08/25 Componente para crear una nueva película
@Component({
  selector: 'crear-pelicula',
  standalone: true,
  imports: [CommonModule, FormsModule, ListaPeliculasComponent, RouterOutlet],
  templateUrl: './crear-pelicula.component.html',
  styleUrl: './crear-pelicula.component.scss'
})
export class CrearPeliculaComponent {
public title = 'Crear Película';
public pelicula: Pelicula = {
    id: 0,
    nombre: '',
    descripcion: '',
    duracion: 0,
    rutaImagen: '',
    clasificacion: 0,
    fechaCreacion: '',
    categoriaID: 0
  };
//public color:string;
//public color_seleccionado:string;

// Inicializando pelicula2 con valores predeterminados
// Puedes cambiar estos valores según tus necesidades
// Si necesitas que pelicula2 sea opcional, puedes inicializarla en el constructor o ngOnInit
// o dejarla como undefined y asignarle un valor más adelante.
constructor(private peliculaService: PeliculaService) {}
archivoSeleccionado: File | null = null;

onArchivoSeleccionado(event: any) {
  if (event.target.files.length > 0) {
    this.archivoSeleccionado = event.target.files[0];
  }
}

crearPelicula() {
  if (
    this.pelicula.nombre &&
    this.pelicula.descripcion &&
    this.pelicula.duracion &&
    this.pelicula.clasificacion &&
    this.pelicula.categoriaID
  ) {
    this.peliculaService.crearPelicula(this.pelicula, this.archivoSeleccionado).subscribe({
      next: (response) => {
        console.log('Película creada exitosamente:', response);
        this.pelicula = {
          id: 0,
          nombre: '',
          descripcion: '',
          duracion: 0,
          rutaImagen: '',
          clasificacion: 0,
          fechaCreacion: '',
          categoriaID: 0
        };
        this.archivoSeleccionado = null;
      },
      error: (error) => {
        console.error('Error al crear la película:', error);
      }
    });
  } else {
    console.error('Por favor, completa todos los campos antes de crear una película.');
  }
}

}