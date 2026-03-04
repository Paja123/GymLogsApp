import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { TrainingType } from '../../../../shared/models/training-type.enum';
import { TrainingService } from '../../services/training.service';
import { TrainingSession } from '../../../../shared/models/training-session.model';
import { Router } from '@angular/router';

function notInFutureValidator(control: AbstractControl): ValidationErrors | null {
  const val = control.value;
  if (!val) return null;

  // handle date-only "YYYY-MM-DD" and datetime-local "YYYY-MM-DDTHH:MM"
  let selected: Date;
  if (/^\d{4}-\d{2}-\d{2}$/.test(val)) {
    const [y, m, d] = val.split('-').map(Number);
    selected = new Date(y, m - 1, d, 23, 59, 59, 999);
  } else {
    selected = new Date(val);
  }

  const now = new Date();
  return selected.getTime() > now.getTime() ? { futureDate: true } : null;
}
@Component({
  selector: 'app-training-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './training-form.html',
  styleUrl: './training-form.css',
})
export class TrainingForm  {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private trainingService = inject(TrainingService);

  trainingTypes = Object.values(TrainingType).filter(value => typeof value === 'string') as string[];

   maxDateTime = this.getLocalDateTimeString();

  trainingForm = this.fb.nonNullable.group({
    trainingType: [TrainingType.Cardio, Validators.required],
    duration: [0, [Validators.required, Validators.min(1), Validators.pattern("^[0-9]*$")]],
    intensityLevel: [5, [Validators.min(1), Validators.max(10)]],
    tirednessLevel: [5, [Validators.min(1), Validators.max(10)]],
    caloriesBurned: [0, [Validators.min(0), Validators.pattern("^[0-9]*$")]],
    date: [this.getNowForInput(), Validators.required],
    notes: ['', Validators.maxLength(300)],
  });

  get f(){
    return this.trainingForm.controls;
  }

  private getNowForInput(): string {
    const now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    return now.toISOString().slice(0, 16);
  }
  private getLocalDateTimeString(): string {
    const d = new Date();
    d.setMinutes(d.getMinutes() - d.getTimezoneOffset());
    return d.toISOString().slice(0, 16);
  }


   onSubmit() {
    if (this.trainingForm.invalid) {
      this.trainingForm.markAllAsTouched();
      return;
    }
    const request: TrainingSession = {
      ...this.trainingForm.getRawValue(),
    };

    this.trainingService.createTrainingSession(request).subscribe({
        next: () => this.router.navigate(["/all-trainings"])
      });
  };

      
    }