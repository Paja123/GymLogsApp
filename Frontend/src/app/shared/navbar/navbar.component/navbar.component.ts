import { Component, inject } from '@angular/core'; 
import { Router } from '@angular/router';
import { AuthService } from '../../../features/auth/services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  private auth = inject(AuthService); 
  private router = inject(Router); 
  loading = false; 

  logout(): void {
    this.loading = true;
    this.auth.logout().subscribe({
      next: () => {
        this.loading = false;
        this.router.navigateByUrl('/login');
      },
      error: () => {
        this.loading = false;
        this.router.navigateByUrl('/login');
      }
    });
  }
}

