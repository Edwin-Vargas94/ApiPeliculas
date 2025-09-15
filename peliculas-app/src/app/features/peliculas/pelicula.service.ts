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

  //private apiUrl = `${environment.apiUrl}/api/${environment.apiVersion}/peliculas`;


  constructor(private http: HttpClient) {}

  obtenerPeliculas(pageNumber: number = 1, pageSize: number = 4): Observable<any> {
  const params = { pageNumber, pageSize };
  return this.http.get(this.apiUrl, { params });
}

crearPelicula(pelicula: Pelicula, archivo: File | null): Observable<any> {
  const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkVkd2luRyIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTc1NTM1ODE2MiwiZXhwIjoxNzU1OTYyOTYyLCJpYXQiOjE3NTUzNTgxNjJ9.zLRxZ__RR4hny6dxx2y9Rm0YscQ71ePfx8jUknhkV5g';

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
  const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkVkd2luRyIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTc1NTM1ODE2MiwiZXhwIjoxNzU1OTYyOTYyLCJpYXQiOjE3NTUzNTgxNjJ9.zLRxZ__RR4hny6dxx2y9Rm0YscQ71ePfx8jUknhkV5g';

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

}