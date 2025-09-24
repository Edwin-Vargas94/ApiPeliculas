import { Component, OnInit } from "@angular/core";
import { Router, RouterLink } from "@angular/router";
import { AuthService } from '@/app/services/auth.service';
import { CommonModule } from "@angular/common";

@Component({
  standalone: true,
  selector: "home",
  imports: [RouterLink, CommonModule],
  templateUrl: "./home.component.html",
  styleUrls: ["./home.component.scss"],
})
export class HomeComponent implements OnInit {
  title = "Página de Inicio";
  isLoggedIn = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.checkAuthStatus();
  }

  checkAuthStatus() {
    this.isLoggedIn = this.authService.isAuthenticated();
    console.log('Estado de autenticación:', this.isLoggedIn);
    console.log('Token actual:', this.authService.getToken());
  }

  logout() {
    this.authService.logout();
    this.isLoggedIn = false;
    console.log('Sesión cerrada');
  }
}