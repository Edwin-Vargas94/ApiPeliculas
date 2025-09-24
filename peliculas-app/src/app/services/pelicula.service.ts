import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '/Users/edwinvargas/Desktop/Api _Curso/ApiPeliculas/peliculas-app/src/environments/environment'; // ajusta la ruta si es necesario

export interface Pelicula {
  id: number;
  nombre: string;
  descripcion: string;
  duracion: number;
  rutaImagen: string;
  rutaLocalImagen?: string;
  clasificacion: number;
  fechaCreacion: string;
  categoriaID: number;
}

@Injectable({
  providedIn: 'root',
})
export class PeliculaService {
  private apiUrl = `${environment.apiUrl}/api/v1/peliculas`;

  constructor(private http: HttpClient) {}

  obtenerPeliculas(pageNumber: number = 1, pageSize: number = 4): Observable<any> {
    const params = { pageNumber, pageSize };
    return this.http.get(this.apiUrl, { params });
  }

  crearPelicula(pelicula: Pelicula, archivo: File | null): Observable<any> {
    const formData = new FormData();
    formData.append('Nombre', pelicula.nombre);
    formData.append('Descripcion', pelicula.descripcion);
    formData.append('Duracion', pelicula.duracion.toString());
    formData.append('Clasificacion', pelicula.clasificacion.toString());
    formData.append('CategoriaID', pelicula.categoriaID.toString());

    if (archivo) {
      formData.append('Imagen', archivo, archivo.name);
    }

    // El interceptor se encarga de agregar el token automáticamente
    return this.http.post(this.apiUrl, formData);
  }

  actualizarPelicula(pelicula: any, imagen?: File): Observable<any> {
    const formData = new FormData();
    formData.append('Id', pelicula.id.toString());
    formData.append('Nombre', pelicula.nombre);
    formData.append('Descripcion', pelicula.descripcion);
    formData.append('Duracion', pelicula.duracion.toString());
    formData.append('Clasificacion', pelicula.clasificacion);
    formData.append('CategoriaID', pelicula.categoriaID.toString());

    if (imagen) {
      formData.append('Imagen', imagen);
    }

    // El interceptor se encarga de agregar el token automáticamente
    return this.http.patch(`${this.apiUrl}/${pelicula.id}`, formData);
  }

  getPeliculaPorId(id: number): Observable<Pelicula> {
    // El interceptor se encarga de agregar el token automáticamente
    return this.http.get<Pelicula>(`${this.apiUrl}/${id}`);
  }

  eliminarPelicula(id: number): Observable<any> {
    // El interceptor se encarga de agregar el token automáticamente
    return this.http.delete(`${this.apiUrl}/${id}`);
  }
}