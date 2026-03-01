import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { ReactiveFormsModule, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { TrainingType } from '../../../../shared/models/training-type.enum';
import { TrainingService } from '../../services/training.service';
import { TrainingSession } from '../../../../shared/models/training-session.model';

@Component({
  selector: 'app-training-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './training-form.html',
  styleUrl: './training-form.css',
})
export class TrainingForm  {
  private fb = inject(FormBuilder);
  private trainingService = inject(TrainingService);

  trainingTypes = Object.values(TrainingType).filter(value => typeof value === 'string') as string[];

  trainingForm = this.fb.nonNullable.group({
    trainingType: [TrainingType.Cardio, Validators.required],
    duration: [0, [Validators.required, Validators.min(1)]],
    intensityLevel: [5, [Validators.min(1), Validators.max(10)]],
    tirednessLevel: [5, [Validators.min(1), Validators.max(10)]],
    caloriesBurned: [0, Validators.min(0)],
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
  // constructor(
  //   private fb =  inject(FormBuilder),
  //   private trainingService =  inject(TrainingService)
  // ){
    //  this.trainingForm = this.fb.group({
    //   trainingType: [null, Validators.required],
    //   duration: [0, [Validators.required, Validators.min(1)]],
    //   intensityLevel: [5, [Validators.required, Validators.min(1), Validators.max(10)]],
    //   tirednessLevel: [5, [Validators.required, Validators.min(1), Validators.max(10)]],
    //   caloriesBurned: [null, Validators.min(0)],
    //   date: [new Date().toISOString().substring(0, 16), Validators.required],
    //   notes: ['', Validators.maxLength(300)],
    //   });
  // }

   onSubmit() {
    if (this.trainingForm.valid) {
      console.log('Form Data:', this.trainingForm.value);
        const request: TrainingSession = {
    ...this.trainingForm.getRawValue(),
    userId: 'userId from jwt later, now still works from model with default value'
  };

      this.trainingService.createTrainingSession(request).subscribe({
        next: (res) => console.log('Saved successfully!', res),
        error: (err) => console.error('Error saving training session:', err)
      });
    }
  }
}
