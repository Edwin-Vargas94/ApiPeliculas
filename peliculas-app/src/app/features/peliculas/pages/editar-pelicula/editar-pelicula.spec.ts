import { ComponentFixture, TestBed } from '@angular/core/testing';

// If the component file is named 'editar-pelicula.component.ts', update the import as follows:
import { EditarPeliculaComponent } from './editar-pelicula.component';

describe('EditarPelicula', () => {
  let component: EditarPeliculaComponent;
  let fixture: ComponentFixture<EditarPeliculaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EditarPeliculaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EditarPeliculaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
