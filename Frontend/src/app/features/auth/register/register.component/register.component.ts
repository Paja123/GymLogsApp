import { Component, inject, signal } from '@angular/core';
import{ FormBuilder, ReactiveFormsModule, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-register.component',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthService);
  private router = inject(Router);
  
  loading = signal(false); 
  error = signal<string | null>(null); 
  
   // Custom validator: trims input and returns granular errors 
  private static passwordComplexity(control: AbstractControl): ValidationErrors | null {
    // const raw = control.value; 
    // if (raw == null || raw === '') return null;
    // const value = String(raw).trim(); 
    // const errors: ValidationErrors = {}; 
    // if (value.length < 6) {
    //   errors['minlength'] = { requiredLength: 6, actualLength: value.length }; 
    // }
    // if (!/[A-Z]/.test(value)) { errors['missingUpper'] = true; } 
    // if (!/\d/.test(value)) { errors['missingDigit'] = true; }
    // if (!/[\W_]/.test(value)) { errors['missingSpecial'] = true; } 

    // return Object.keys(errors).length ? errors : null; 
    const value = control.value;
  if (!value) return null;

  const errors: ValidationErrors = {};

  if (value.length < 6) errors['minlength'] = true;
  if (!/[A-Z]/.test(value)) errors['missingUpper'] = true;
  if (!/[0-9]/.test(value)) errors['missingDigit'] = true;
  if (!/[^a-zA-Z0-9]/.test(value)) errors['missingSpecial'] = true;

  return Object.keys(errors).length ? errors : null;
  }

   
  form = this.fb.group({
    firstName: ['', [Validators.required]], 
    lastName: ['', [Validators.required]], 
    userName: ['', [Validators.required, 
    Validators.minLength(3)]], 
    email: ['', [Validators.required, Validators.email]], 
    password: ['', [Validators.required, RegisterComponent.passwordComplexity]] 
  });

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched(); 
      return;
    } 

    const firstName = this.form.get('firstName')?.value ?? ''; 
    const lastName = this.form.get('lastName')?.value ?? ''; 
    const userName = this.form.get('userName')?.value ?? ''; 
    const email = this.form.get('email')?.value ?? ''; 
    const password = this.form.get('password')?.value ?? ''; 

    if (!firstName || !lastName || !userName || !email || !password) {
      this.error.set('All fields are required.');
      return;
    }

    const payload = {
      firstName: String(firstName),
      lastName: String(lastName),
      userName: String(userName),
      email: String(email),
      password: String(password)
    };

    this.loading.set(true); 
    this.error.set(null);

    this.auth.register(payload).subscribe({
      next: res => {
        this.loading.set(false); 
        this.router.navigateByUrl('/login');
       },
        error: () => {
          this.loading.set(false); 
          this.error.set('Registration failed. Try again later.');
        }
    });
  }
}
