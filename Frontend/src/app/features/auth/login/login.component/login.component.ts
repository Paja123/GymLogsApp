import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { LoginRequest } from '../../models/auth.models';

@Component({
  selector: 'app-login.component',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  loading = signal(false);
  error = signal<string | null>(null);
  showPassword = signal(false);
 
  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  submit(): void{
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const email = this.form.get('email')?.value ?? '';
    const password = this.form.get('password')?.value ?? '';

    if (!email || !password) {
      this.error.set('Email and password are required.');
      return;
    }

    const payload: LoginRequest = { email: String(email), password: String(password)};

    this.loading.set(true);
    this.error.set(null);

    this.authService.login(payload).subscribe({
      next: res => {
        this.loading.set(false);
        this.router.navigate(['/all-trainings'])
      },
      error: () => {
        this.loading.set(false);
        this.error.set('Login failed. Please check your credentials.');
      }
    });
  }
  togglePassword(): void{
    this.showPassword.set(!this.showPassword());
  }


}
