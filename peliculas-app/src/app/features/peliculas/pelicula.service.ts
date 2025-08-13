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

crearPelicula(pelicula: Pelicula): Observable<any> {
  const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkVkd2luRyIsInJvbGUiOiJBZG1pbiIsIm5iZiI6MTc1NTAyNzkwMywiZXhwIjoxNzU1NjMyNzAzLCJpYXQiOjE3NTUwMjc5MDN9.tu_je79EraZIcHR-KvElJeeS_RhJvR32AQl8rZtiHSM';

  const headers = {
    Authorization: `Bearer ${token}`,
    'Content-Type': 'application/json'
  };

  return this.http.post(this.apiUrl, pelicula, { headers });
}

}
