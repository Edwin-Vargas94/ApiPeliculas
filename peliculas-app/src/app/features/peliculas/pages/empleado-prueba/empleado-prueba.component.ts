import { Component } from "@angular/core";

@Component({
  selector: "empleado-prueba",
  templateUrl: "./empleado-prueba.component.html",
  styleUrls: ["./empleado-prueba.component.scss"],
})
export class EmpleadpPruebaComponent {
  title = "Empleado Prueba";

public nombre:string;
public edad:number;
public mayorDeEdad: boolean = true;

constructor() {
  this.nombre = "Edwin Vargas";
  this.edad = 30;

  //this.holaMundo();
  }

  // método que se ejecuta al iniciar el componente
  ngOnInit() {

  // Variables y alcance

  var dos = 15;// alcance de función
  let tres = 20;// alncance de bloque

   //this.cambiarNombre();
   //alert(this.nombre + " " + this.edad);
  }

  // Método para cambiar el nombre
  cambiarNombre() {
    this.nombre = "Gibran Vargas";
  }
  
  holaMundo(){
    alert("Hola Mundo");
  }
}   