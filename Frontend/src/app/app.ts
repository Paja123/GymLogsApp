import { Component, OnInit, inject } from '@angular/core';
import { RouterOutlet, Router, NavigationEnd, ActivatedRoute } from '@angular/router';
import { NavbarComponent } from './shared/navbar/navbar.component/navbar.component';
import { AuthService } from './features/auth/services/auth.service';
import { map, filter } from 'rxjs';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, NavbarComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private authService = inject(AuthService);
  showNavbar = true;

  constructor(private router: Router, private route: ActivatedRoute) {
    this.router.events.pipe(
    filter(e => e instanceof NavigationEnd),
    map(() => {
      let r = this.route.root;
      while (r.firstChild) r = r.firstChild;
      return r.snapshot.data;
    })
    ).subscribe(data => {
      this.showNavbar = !data?.['hideNavbar'];
    });

  }
  ngOnInit() {
    this.authService.fetchCurrentUser().subscribe();
    
    let r = this.route.root;
    while (r.firstChild) r = r.firstChild;
    this.showNavbar = !r.snapshot.data?.['hideNavbar'];
  }

   
}

