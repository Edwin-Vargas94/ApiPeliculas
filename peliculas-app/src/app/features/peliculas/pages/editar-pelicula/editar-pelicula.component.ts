import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, NgForm, NgModel } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PeliculaService } from '../../pelicula.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-editar-pelicula',
  imports: [MatFormFieldModule, MatInputModule, ReactiveFormsModule, FormsModule],
  templateUrl: './editar-pelicula.component.html',
  styleUrl: './editar-pelicula.component.scss'
})
export class EditarPeliculaComponent {
pelicula: any = {
    id: 0,
    nombre: '',
    descripcion: '',
    duracion: 0,
    clasificacion: 0,
    categoriaID: 0
  };

  imagenSeleccionada?: File;

  constructor(private peliculaService: PeliculaService,
    private route: ActivatedRoute,
  ) {}

ngOnInit(): void {
  const id = Number(this.route.snapshot.paramMap.get('id')); // OJO: 'id' en minúscula
  console.log("ID capturado desde ruta:", id); // 👈 imprime en consola
  if (id) {
    this.peliculaService.getPeliculaPorId(id).subscribe({
      next: (data) => this.pelicula = data,
      error: (err) => console.error("Error cargando película:", err)
    });
  }
}

  onFileSelected(event: any) {
    this.imagenSeleccionada = event.target.files[0];
  }

  actualizar() {
    this.peliculaService.actualizarPelicula(this.pelicula, this.imagenSeleccionada)
      .subscribe({
        next: () => alert('Película actualizada correctamente'),
        error: err => console.error('Error al actualizar', err)
      });
  }
}

