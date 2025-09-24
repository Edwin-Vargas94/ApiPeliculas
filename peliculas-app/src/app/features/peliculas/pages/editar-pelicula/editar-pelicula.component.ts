import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { PeliculaService } from '../../../../services/pelicula.service';

interface Pelicula {
  id: number;
  nombre: string;
  descripcion: string;
  duracion: number;
  clasificacion: string;
  categoriaID: number;
}

@Component({
  selector: 'app-editar-pelicula',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './editar-pelicula.component.html',
  styleUrl: './editar-pelicula.component.scss'
})
export class EditarPeliculaComponent implements OnInit {
  
  pelicula: Pelicula = {
    id: 0,
    nombre: '',
    descripcion: '',
    duracion: 0,
    clasificacion: '',
    categoriaID: 0
  };

  // 👇 Catálogo de clasificaciones
  clasificaciones = [
    { id: 1, nombre: 'G - Audiencia General' },
    { id: 2, nombre: 'PG - Se sugiere supervisión de padres' },
    { id: 3, nombre: 'PG-13 - Padres precaución (menores de 13)' },
    { id: 4, nombre: 'R - Restringida' },
    { id: 5, nombre: 'NC-17 - Solo adultos' }
  ];

  // 👇 Catálogo de catégorias
  categorias = [
    { id: 1, nombre: 'Acción' },
    { id: 2, nombre: 'Comedia' },
    { id: 3, nombre: 'Drama' },
    { id: 4, nombre: 'Terror' },
    { id: 5, nombre: 'Ciencia Ficción' },
    { id: 6, nombre: 'Romance' },
    { id: 7, nombre: 'Thriller' },
    { id: 8, nombre: 'Aventura' }
  ];

  imagenSeleccionada?: File;
  loading: boolean = false;

  constructor(
    private peliculaService: PeliculaService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    console.log("ID capturado desde ruta:", id);
    
    if (id && id > 0) {
      this.cargarPelicula(id);
    }
  }

  cargarPelicula(id: number): void {
    this.loading = true;
    this.peliculaService.getPeliculaPorId(id).subscribe({
      next: (data) => {
        this.pelicula = {
          ...data,
          clasificacion: String(data.clasificacion)
        };
        this.loading = false;
        console.log("Película editada:", data);
      },
      error: (err) => {
        console.error("Error cargando película:", err);
        this.loading = false;
        alert('Error al editar la película');
      }
    });
  }

  onArchivoSeleccionado(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const archivo = input.files[0];
      
      // Validar tipo de archivo
      const allowedTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
      if (!allowedTypes.includes(archivo.type)) {
        alert('Por favor, selecciona un archivo de imagen válido (JPG, PNG, GIF, WEBP)');
        input.value = '';
        return;
      }

      // Validar tamaño del archivo (ejemplo: máximo 5MB)
      const maxSize = 5 * 1024 * 1024; // 5MB en bytes
      if (archivo.size > maxSize) {
        alert('El archivo es demasiado grande. El tamaño máximo es 5MB.');
        input.value = '';
        return;
      }

      this.imagenSeleccionada = archivo;
      console.log('Archivo seleccionado:', archivo);
    }
  }

  actualizar(): void {
    if (this.validarFormulario()) {
      this.loading = true;

      console.log('Datos a actualizar:', this.pelicula);
      console.log('Imagen seleccionada:', this.imagenSeleccionada);

      this.peliculaService.actualizarPelicula(this.pelicula, this.imagenSeleccionada)
        .subscribe({
          next: (response) => {
            console.log('Película actualizada exitosamente:', response);
            alert('Película actualizada correctamente');
            this.loading = false;
            // Opcional: redirigir o limpiar formulario
          },
          error: (err) => {
            console.error('Error al actualizar película:', err);
            alert('Error al actualizar la película');
            this.loading = false;
          }
        });
    }
  }

  private validarFormulario(): boolean {
    if (!this.pelicula.nombre.trim()) {
      alert('El nombre es requerido');
      return false;
    }

    if (!this.pelicula.descripcion.trim()) {
      alert('La descripción es requerida');
      return false;
    }

    if (this.pelicula.duracion <= 0) {
      alert('La duración debe ser mayor a 0');
      return false;
    }

    if (!this.pelicula.clasificacion.trim()) {
      alert('La clasificación es requerida');
      return false;
    }

    if (this.pelicula.categoriaID <= 0) {
      alert('Debe seleccionar una categoría válida');
      return false;
    }

    return true;
  }

  cancelar(): void {
    if (confirm('¿Estás seguro de que deseas cancelar? Los cambios no guardados se perderán.')) {
      // Aquí puedes navegar a la lista de películas o página anterior
      window.history.back();
    }
  }
}