import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
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

  //private apiUrl = `${environment.apiUrl}/api/${environment.apiVersion}/peliculas`;


  constructor(private http: HttpClient) {}

  obtenerPeliculas(pageNumber: number = 1, pageSize: number = 4): Observable<any> {
  const params = { pageNumber, pageSize };
  return this.http.get(this.apiUrl, { params });
}

crearPelicula(pelicula: Pelicula, archivo: File | null): Observable<any> {
  const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImVkd2luIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzU4MTc2Mzc2LCJleHAiOjE3NTg3ODExNzYsImlhdCI6MTc1ODE3NjM3Nn0.P9iUbEOLgUo00g1kcpZ3tZtOggNJBVRuUOLYOhRHQEc';

  const headers = {
    Authorization: `Bearer ${token}`,
    // No pongas Content-Type porque el navegador lo establece automáticamente cuando usas FormData
  };

  const formData = new FormData();

  formData.append('Nombre', pelicula.nombre);
  formData.append('Descripcion', pelicula.descripcion);
  formData.append('Duracion', pelicula.duracion.toString());
  formData.append('Clasificacion', pelicula.clasificacion.toString());  // o el valor string si usas enum como texto
  formData.append('CategoriaID', pelicula.categoriaID.toString());

  if (archivo) {
    formData.append('Imagen', archivo, archivo.name);
  }

  return this.http.post(this.apiUrl, formData, { headers });
}

// ejemplo en servicio
actualizarPelicula(pelicula: any, imagen?: File): Observable<any> {
  const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImVkd2luIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzU4MTc2Mzc2LCJleHAiOjE3NTg3ODExNzYsImlhdCI6MTc1ODE3NjM3Nn0.P9iUbEOLgUo00g1kcpZ3tZtOggNJBVRuUOLYOhRHQEc';

  const headers = {
    Authorization: `Bearer ${token}`,
    // No pongas Content-Type porque el navegador lo establece automáticamente cuando usas FormData
  };  
  const formData = new FormData();

    // Los nombres deben coincidir con tu ActualizarPeliculaDto en C#
    formData.append('Id', pelicula.id.toString());
    formData.append('Nombre', pelicula.nombre);
    formData.append('Descripcion', pelicula.descripcion);
    formData.append('Duracion', pelicula.duracion.toString());
    formData.append('Clasificacion', pelicula.clasificacion);
    formData.append('CategoriaID', pelicula.categoriaID.toString());

    // Solo enviamos la imagen si hay una nueva
    if (imagen) {
      formData.append('Imagen', imagen);
    }

    return this.http.patch(`${this.apiUrl}/${pelicula.id}`, formData, { headers });
  }


getPeliculaPorId(id: number): Observable<Pelicula> {
  return this.http.get<Pelicula>(`${this.apiUrl}/${id}`);

}

// En pelicula.service.ts
eliminarPelicula(id: number): Observable<any> {
  const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImVkd2luIiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzU4MTc2Mzc2LCJleHAiOjE3NTg3ODExNzYsImlhdCI6MTc1ODE3NjM3Nn0.P9iUbEOLgUo00g1kcpZ3tZtOggNJBVRuUOLYOhRHQEc';
  
  const headers = new HttpHeaders({
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  });

  return this.http.delete(`${this.apiUrl}/${id}`, { headers });
}

}