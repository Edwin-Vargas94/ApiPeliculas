import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
 templateUrl: './footer.component.html',
  styles: [`
    .footer {
      background-color: #eeeeee;
      padding: 1rem;
      text-align: center;
      font-size: 0.9rem;
      color: #444;
    }
  `]
})
export class FooterComponent {
  currentYear = new Date().getFullYear();
}

