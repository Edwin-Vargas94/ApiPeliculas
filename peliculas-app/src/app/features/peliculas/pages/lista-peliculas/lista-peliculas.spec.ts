import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListaPeliculasComponent } from './lista-peliculas.component';

describe('ListaPeliculas', () => {
  let component: ListaPeliculasComponent;
  let fixture: ComponentFixture<ListaPeliculasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListaPeliculasComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListaPeliculasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
