import { Component } from "@angular/core";
import { RouterLink } from "@angular/router";

@Component({
  standalone: true,
  selector: "home",
  imports: [RouterLink],
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"],
})
export class HomeComponent {
  title = "Página de Inicio";
}   